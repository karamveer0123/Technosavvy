jQuery("#carousel").owlCarousel({
    autoplay:false,
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
        items: 2
      },
  
      1366: {
        items: 2
      }
    }
  });



