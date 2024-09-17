
  $('.owl-carousel').owlCarousel({
    loop:true,
    margin:10,
    dots:false,
    nav:true,
      autoplay: false,
      responsiveClass: true,
      smartSpeed: 800,
      autoplayTimeout: 7000,
    responsive:{
        0:{
            items:1
        },
        600:{
            items:1
        },
        700: {
            items: 2
        },
        1000:{
            items:3
        }
    }
})


jQuery("#carousel1").owlCarousel({
    autoplay: false,
    rewind: true, /* use rewind if you don't want loop */
    margin: 20,

    /*
   animateOut: 'fadeOut',
   animateIn: 'fadeIn',
   */
    responsiveClass: true,
    autoHeight: false,
    autoplayTimeout: 7000,
    smartSpeed: 800,
    margin: 50,
    nav: true,
    responsive: {
        0: {
            items: 1
        },

        600: {
            items: 1
        },

        1024: {
            items: 3
        },

        1366: {
            items: 3
        }
    }
});


