/*
 * Библиотека для авторизации
 */
;
(function (global, $) {
    'use strict';

    var Auth = function () {
        return new Auth.init();
    };

    /*
    * Подготавливает массив объектов к отправки
    *
    * @param arr: массив объектов
    * @return object
    */
    var prepareToSend = function (arr) {
        var obj = {};
        for (var i = 0, length = arr.length; i < length; i++) {
            obj[arr[i].name] = arr[i].value;
        }
        return obj;
    };

    Auth.prototype = {
        /*
        * Проверяет авторизован ли пользователь
        */
        checkAuthorization: function () {
            if (localStorage['user']) {
                var fromLocal = JSON.parse(localStorage['user']);
                var hash = CryptoJS.HmacSHA1(fromLocal.idUser + fromLocal.secret, fromLocal.secret.toString()).toString();

                $.ajax({
                    type: "GET",
                    url: '/api/accounts/auth',
                    headers: { 'idUser': fromLocal.idUser, 'hash': hash },
                    beforeSend: function (xhr) {
                        var token = fromLocal.secret;
                        xhr.setRequestHeader("Authorization", "Bearer " + token);
                    },
                    success: function (data, statusText, xhr) {
                        window.location = '/home/theme/#/theme';
                    },
                    error: function (xhr, textStatus, error) {
                        window.location= '#/login';
                    }
                });
            }
        },
        /*
        * Регистрация пользователя
        *
        * @param obj: объект пришедший с валидной формы регистрации
        */
        registration: function (obj) {
            $(".infoAuth").empty();
            $.ajax({
                url: "api/Accounts/create/",
                type: "POST",
                dataType: "json",
                data: JSON.stringify({
                    "name": obj.name,
                    "email": obj.email,
                    "password": obj.pass,
                    "confirmPassword": obj.pass_confirmation,
                    "category": obj.category
                }),
                contentType: "application/json",
                statusCode: {
                    200: function (data, statusText, xhr) {
                        var massege = $(".infoAuth");
                        massege.html('<p class="success">Акаунт створено!</p>');
                        massege.slideDown();
                    },
                    404: function (data, statusText, error) {
                        var arrayOfError = JSON.parse(data.getResponseHeader('error'));
                        $(arrayOfError).each(function (k, v) {
                            var massege = $(".infoAuth");
                            massege.html('<p class="error">' + v + '</p>');
                            massege.slideDown();
                        });
                    },
                    400: function (data, statusText, error) {
                        var massege = $(".infoAuth");
                        massege.html('<p class="error">' + error + '</p>');
                        massege.slideDown();
                    }
                }
            });
        },
        /*
         * Авторизация пользователя
         *
         * @param obj: объект пришедший с валидной формы входа
         */
        login: function (obj) {
            var tokenKey = "tokenInfo";
            var userId = "userId";
            $(".infoAuth").empty();
            $.ajax({
                url: '/oauth/token',
                type: "PUT",
                data: { "UserName": obj.email, "password": obj.pass, "grant_type": "password" },
                statusCode: {
                    200: function (data, statusText, xhr) {
                        var values = xhr.getResponseHeader('Keys');
                        localStorage['user'] = JSON.stringify({ "idUser": values, "secret": data.access_token});
                        //sessionStorage.setItem(tokenKey, data.access_token);
                        //sessionStorage.setItem(userId, values);
                        window.location = '/home/theme';
                    },
                    400: function (data, statusText, error) {
                        var massege = $(".infoAuth");
                        massege.html('<p class="error">' + error + '</p>');
                        massege.slideDown();
                    },
                    404: function (data, statusText, error) {
                        var arrayOfError = JSON.parse(data.getResponseHeader('error'));
                        $(arrayOfError).each(function (k, v) {
                            var massege = $(".infoAuth");
                            massege.html('<p class="error">' + v + '</p>');
                            massege.slideDown();
                        });
                    }
                }
            });
        }
    };

    Auth.init = function () {
        var _this = this;

        //подключение модуля валидации и безопасности
        $.validate({
            modules: 'security'
        });

        //отключаю отправку формы по нажанию на submit
        $('form').on('submit', function () {
            return false;
        });

        //получение данных из формы
        $('input[type=submit]').on('click', function (e) {
            setTimeout(function () {
                var targetForm = $(e.target).parents('form');
                if (!targetForm.hasClass('has-error')) {
                    if (targetForm.attr('id') === 'registration') {
                        _this.registration(prepareToSend(targetForm.serializeArray()));
                    } else if (targetForm.attr('id') === 'login')
                        _this.login(prepareToSend(targetForm.serializeArray()));
                }
            }, 0);
        });
    };

    Auth.init.prototype = Auth.prototype;

    global.uawcapp.Auth = Auth;

})(window, jQuery);
