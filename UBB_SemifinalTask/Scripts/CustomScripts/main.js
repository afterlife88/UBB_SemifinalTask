$( document ).ready(function() {

    $('.template__design').click(function(){
        $(this).addClass('active');
        $('.template__content').removeClass('active');
        // Hide elements panel
        $('.elements').hide();
        // Show design panel
        $('.design__item').show();
    });

    $('.template__content').click(function(){
        $(this).addClass('active');
        $('.template__design').removeClass('active');
        // Hide design panel
        $('.design__item').hide();
        // Show elements panel
        $('.elements').show();
    });

    $('.design__title').click(function(){
        var block = $(this);
        if (block.parent().is('.open')) {
            block.parent().removeClass('open');
            block.parent().find('.setting').slideUp();
        } else {
            block.toggleClass('arrow');
            block.parent().find(".setting").slideToggle();
        }
    });

    $('.dropdown__title').click(function(){
        var block = $(this);
        if (block.parent().is('.open')) {
            block.parent().removeClass('open');
            block.parent().find('.interior').slideUp();
        } else {
            block.toggleClass('arrow');
            block.parent().find(".interior").slideToggle();
        }
    });
});
