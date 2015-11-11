/*
 * Библиотека для обработки стилей
 */
;
(function (global, $) {
    'use strict';

    var Style = function () {
        return new Style.init();
    };

    // Объект соответствий значения name с css селекторами
    var css = {
        'font': '.page__sheet',
        'h1': '.page__sheet div[data-type=h1] div.adding_content h1',
        'h1-size': '.page__sheet div[data-type=h1]',
        'h2': '.page__sheet div[data-type=h2] div.adding_content h2',
        'h2-size': '.page__sheet div[data-type=h2]',
        'excerption': '.page__sheet div[data-type=excerption] div.adding_content q',
        'excerption-size': '.page__sheet div[data-type=excerption]',
        'text': '.page__sheet div[data-type=text] div.adding_content p, div[data-type=text] div.adding_content p div',
        'text-size': '.page__sheet div[data-type=text]',
        'picture-text': '.page__sheet div[data-type=picture-text] .top__right h1',
        'picture-text-border': '.page__sheet div[data-type=picture-text]'
    };

    /*
     * Инициализация стилей. Получает с сервера стили в виде JSON.
     *
     * @return boolean или object
     */
      var initStyle = function (data) {
        if (localStorage['user']) {
            var fromLocal = JSON.parse(localStorage['user']);
            var idUser = fromLocal.idUser;
            var hash = CryptoJS.HmacSHA1(fromLocal.idUser + fromLocal.secret, fromLocal.secret.toString()).toString();
            delete hash.access_token;
            $.ajax({
                type: 'GET',
                url: '/api/css/getStyle',
               // data: sendData,
                dataType: "html",
                headers: { 'AuthorId': idUser, 'hash': hash },
                beforeSend: function (xhr) {
                    var token = fromLocal.secret;
                    xhr.setRequestHeader("Authorization", "Bearer " + token);
                },
                statusCode: {
                    200: function (data) {
                        return JSON.parse(data);
                    },
                    404: function () {
                        return false;
                    }
                }
            });
        }
    };

    /*
     * Преобразовывает массив объектов в строку
     *
     * @param arrObj: массив объектов
     * @return boolean или string
     */
    var getCssString = function (arrObj) {
        if (!arrObj) {
            return false;
        }
        var i, length, content, type, cssString, currentObj, globalStyle = {}, globalStyleType = {}, prop, val;

        for (i = 0, length = arrObj.length; i < length; i++) {
            currentObj = arrObj[i];
            content = currentObj['name'].slice(0, (currentObj['name'].indexOf('[')));
            type = currentObj['name'].slice((currentObj['name'].indexOf('[')) + 1, -1);

            if (!globalStyle[content]) {
                globalStyleType = {};
            }
            globalStyleType[type] = currentObj['value'];
            globalStyle[content] = globalStyleType;
        }
        cssString = '<style>';
        for (prop in globalStyle) {
            if (globalStyle.hasOwnProperty(prop)) {
                cssString += css[prop] + '{';
                for (val in globalStyle[prop]) {
                    if (globalStyle[prop].hasOwnProperty(val)) {
                        cssString += val + ': ' + globalStyle[prop][val] + ';';
                    }
                }
                cssString += '}';
            }
        }
        cssString += '</style>';
        return cssString;
    };

    Style.prototype = {
        /*
         * Устанавливает стили на страницу
         *
         * @param getFormValue: массив объектов
         * @return string
         */
        setStyle: function (getFormValue) {
            var style = getCssString(getFormValue);
            $('.data__style').html(style);
            return style;
        },
        serializeStyle: function (getFormValue) {
            var style = getCssString(getFormValue);
            
            return style;
        },
        /*
         * Сохраняет стили в базе данных
         *
         * @param value: строка стилей
         * @return boolean
         */
        saveStyle: function (value) {
            if (localStorage['user']) {
                var fromLocal = JSON.parse(localStorage['user']);
                var idUser = fromLocal.idUser;
                var hash = CryptoJS.HmacSHA1(idUser + value, fromLocal.secret.toString()).toString();
              //  alert(value);
                $.ajax({
                    method: "POST",
                    dataType: "json",
                    contentType: "application/json",
                    url: '/api/css/setStyle',
                    data: JSON.stringify({ "AuthorId": idUser, "Styles": value, "hash": hash }),
                     beforeSend: function (xhr) {
                         var token = fromLocal.secret;
                         xhr.setRequestHeader("Authorization", "Bearer " + token);
                    },
                    statusCode: {
                        200: function (data) {
                            return true;
                        },
                        404: function () {
                            return false;
                        }
                    }
                });
            } else {
                //если не авторизован
                window.location.href = '/';
            }
        }
    };

    Style.init = function () {
        var _this = this;
        _this.setStyle(initStyle());
    };

    Style.init.prototype = Style.prototype;

    global.uawcapp.Style = Style;

})(window, jQuery);