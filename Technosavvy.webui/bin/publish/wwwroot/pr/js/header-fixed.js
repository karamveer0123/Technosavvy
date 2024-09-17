
window.onscroll = function() {myFunction()};

var header = document.getElementById("myHeader");
var sticky = header.offsetTop;

function myFunction() {
  if (window.pageYOffset > sticky) {
    header.classList.add("sticky");
  } else {
    header.classList.remove("sticky");
  }
}




/********************************************************/

  // notification js

  $(".link").click(function(e){
    e.preventDefault();
    $(".popup").fadeIn(300,function(){$(this).focus();});
  });
  
  $('.close').click(function() {
   $(".popup").fadeOut(300);
  });
  
  $(".popup").on('blur',function(){
    $(this).fadeOut(300);
  });
