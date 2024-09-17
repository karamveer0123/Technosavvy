

// let mybutton = document.getElementById("btn-back-to-top");
// window.onscroll = function () {scrollFunction();};

// function scrollFunction() {
//   if (
//     document.body.scrollTop > 20 ||
//     document.documentElement.scrollTop > 20
//   ) {
//     mybutton.style.display = "block";
//   } else {
//     mybutton.style.display = "none";
//   }
// }
// mybutton.addEventListener("click", backToTop);

// function backToTop() {
//   document.body.scrollTop = 0;
//   document.documentElement.scrollTop = 0;
// }

// ==============


$(document).ready(function(){
  $(window).scroll(function(){
      if ($(this).scrollTop() > 100) {
          $('#btn-back-to-top').fadeIn();
      } else {
          $('#btn-back-to-top').fadeOut();
      }
  });
  $('#btn-back-to-top').click(function(){
      $("html, body").animate({ scrollTop: 0 }, 600);
      return false;
  });
});


