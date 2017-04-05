$(window).on('load', function () {
    //Small timeout to ensure a smooth transition (pure aesthetics!)
    setTimeout(function() {
        $('.preloader').fadeOut("slow");
    }, 500);
});