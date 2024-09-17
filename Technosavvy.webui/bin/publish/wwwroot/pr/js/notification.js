

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
