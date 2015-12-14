/*
 * Библиотека для обработки виджетов
 */
;
(function (global, $) {
    'use strict';

    var Widgets = function () {
        return new Widgets.init();
    };

    //объект соответствий значения data-type с шаблоном
    var shablonObj = {
        'h1': '<div class="hover"><i class="hover__full"></i><ul class="hover__buttons"><li class="hover__item edit"></li><li class="hover__item copy"></li><li class="hover__item delete"></li></ul></div><div class="adding_content"><h1 contenteditable="true">Редагувати текст H1</h1></div>',
        'h2': '<div class="hover"><i class="hover__full"></i><ul class="hover__buttons"><li class="hover__item edit"></li><li class="hover__item copy"></li><li class="hover__item delete"></li></ul></div><div class="adding_content"><h2 contenteditable="true">Редагувати текст H2</h2></div>',
        'excerption': '<div class="hover"><i class="hover__full"></i><ul class="hover__buttons"><li class="hover__item edit"></li><li class="hover__item copy"></li><li class="hover__item delete"></li></ul></div><div class="adding_content"><q contenteditable="true">Редагувати текст цитати</q></div>',
        'text': '<div class="hover"><i class="hover__full"></i><ul class="hover__buttons"><li class="hover__item edit"></li><li class="hover__item copy"></li><li class="hover__item delete"></li></ul></div><div class="adding_content"><p contenteditable="true">Редагувати текст</p></div>',
        'broke-line': '<div class="hover"><i class="hover__full"></i><ul class="hover__buttons"><li class="hover__item edit"></li><li class="hover__item copy"></li><li class="hover__item delete"></li></ul></div><div class="adding_content"><hr></div>',
        'count': '<div class="hover"><i class="hover__full"></i><ul class="hover__buttons"><li class="hover__item edit"></li><li class="hover__item copy"></li><li class="hover__item delete"></li></ul></div><div class="adding_content"><div class="center__right"><h3>Щоб активувати лічильник, додайте лінк на свою сторінку на сайті УББ</h3><div class="center__counter"><div class="center__process"></div></div><button class="center__button">Допомогти</button></div></div>'
    };

    /*
     * Инициализация событий сортировки и мувинга
     */
    var initWidgets = function () {
        var contentDraggable = $(".content_draggable");

        //определяем область сортировки
        $("#sortable").sortable({
            revert: true,
            sort: function (event, ui) {
            },
            handle: '.hover__full'
        });

        //определяем облать двигаемых объектов
        contentDraggable.draggable({
            connectToSortable: "#sortable",
            helper: "clone",
            revert: "invalid",
            stop: function (event, ui) {
            },
            drag: function (event, ui) {
            }
        });
        $("ul, li").disableSelection();
        /*
         contentDraggable.on("dragstop", function (event, ui) {
         $('#sortable li').each(function (k, v) {
         v.style.border = 'none';
         });
         });
         */
        //принимаем при перетаскивании шаблон
        contentDraggable.on("drag", function (event, ui) {
            var dom = ui.helper[0];
            var type = dom.dataset.type;
            if (type && shablonObj.hasOwnProperty(type)) {
                dom.innerHTML = shablonObj[type];
            }
            /*
             $('#sortable div.hover__full').each(function (k, v) {
             v.style.border = '1px solid #AB7E30'
             });
             */
        });
    };

    /*
     * Удаление блока
     *
     * @param element: dom элемент
     */
    var deleteElement = function (element) {
        return element.remove();
    };

    /*
     * Копирование блока
     *
     * @param element: dom элемент
     */
    var copyElement = function (element) {
        return $('#sortable').append(element.clone());
    };

    /*
     * Редактирование блока
     *
     * @param element: dom элемент
     */
    var editElement = function (element) {
        $('.elements').hide();
        $('[class ^= element__]').hide();
        $('.element__' + element.type).show();
    };

    Widgets.prototype = {
        /*
         * Модификация блока
         *
         * @param target: dom элемент
         */
        modifyPage: function (target) {
            switch (true) {
                case target.hasClass('delete'):
                    deleteElement(target.parents('.sort_list'));
                    break;
                case target.hasClass('copy'):
                    copyElement(target.parents('.sort_list'));
                    break;
                case target.hasClass('edit'):
                    editElement(target.parents('.sort_list')[0].dataset);
                    console.log('edit');
                    break;
            }
        },
        /*
         * Модификация блока
         *
         * @param value: string вся страница предпросмотра
         * @boolean
         */
        savePage: function (value, linkOnUbb, css, projName) {
            if (localStorage['user']) {
                var fromLocal = JSON.parse(localStorage['user']);
                var idUser = fromLocal.idUser;
                var hash = CryptoJS.HmacSHA1(idUser + value, fromLocal.secret.toString()).toString();
                $.ajax({
                    method: "POST",
                    dataType: "json",
                    contentType: "application/json",
                    url: '/api/css/setPage',
                    data: JSON.stringify({ "AuthorId": idUser, "Content": value, "hash": hash, "LinkOnUbb": linkOnUbb, "Styles": css, "ProjectName": projName }),
                    beforeSend: function (xhr) {
                        var token = fromLocal.secret;
                        xhr.setRequestHeader("Authorization", "Bearer " + token);
                    },
                    statusCode: {
                        200: function (data) {
                            alert('Збережено успішно!');
                            var item = JSON.parse(JSON.stringify(data));
                            $("#getLink").append('<a href="http://localhost:1711/Home/GetPage/' + item.id + '"style="color: white">Отримати лінк</a>');
                        },
                        404: function () {
                            alert('Виникла помилка при збереженні! Перевірте введені данні!');
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

    Widgets.init = function () {
        var _this = this;
        initWidgets();

    };

    Widgets.init.prototype = Widgets.prototype;

    global.uawcapp.Widgets = Widgets;

})(window, jQuery);
