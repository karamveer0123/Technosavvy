jQuery("#carousel").owlCarousel({
    autoplay:false,
    rewind: true,
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
    },
    
});

jQuery("#carouselML").owlCarousel({
    autoplay: false,
    rewind: true,
    responsiveClass: true,
    autoHeight: false,
    autoplayTimeout: 7000,
    smartSpeed: 800,
    margin: 20,
    stagePadding: 50,
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
    },

});
$(document).ready(function () {    
    var showChar = 90; 
    var ellipsestext = "...";
    var moretext = "Show More";
    var lesstext = "Show Less";

    $('.academyText').each(function () {
        var content = $(this).html();
        console.log('content', content.length);
        if (content.length > showChar) {

            var c = content.substr(0, showChar);
            var h = content.substr(showChar, content.length - showChar);
            console.log('c', c)
            var html = c + '<span class="moreellipses">' + ellipsestext + '&nbsp;</span><span class="morecontent"><span>' + h + '</span>&nbsp;&nbsp;<a href="" class="morelink">' + moretext + '</a></span>';
            $(this).html(html);
        }

    });

    $(".morelink").click(function () {
        if ($(this).hasClass("less")) {
            $(this).removeClass("less");
            $(this).html(moretext);
        } else {
            $(this).addClass("less");
            $(this).html(lesstext);
        }
        $(this).parent().prev().toggle();
        $(this).prev().toggle();
        return false;
    });
});