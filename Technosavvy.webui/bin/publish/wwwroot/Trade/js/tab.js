//  $('.trade').click(function(){  
//        $("#tabred").addClass('trade-active1');
//	  $("#tabGreen").removeClass('trade-active1');
//        $(".trade-btn").removeClass('trade-a1');
//        $(this).parent().find(".trade-btn").addClass('trade-a1');
//     }); 

function changetrdTab(){

	$(".red").addClass("active-a9");
	$(".green").removeClass("active-a9");
	$("#tabred").addClass("tab-active9");
	$("#tabGreen").removeClass("tab-active9");
}
function changetab(){
	$(".red").removeClass("active-a9");
	$(".green").addClass("active-a9");
	$("#tabred").removeClass("tab-active9");
	$("#tabGreen").addClass("tab-active9");
}


function changetrdTab1(){
	$(".red").addClass("active-a93");
	$(".green").removeClass("active-a93");
	$("#tabred1").addClass("tab-active2");
	$("#tabGreen1").removeClass("tab-active2");
}
function changetab1(){
	$(".red").removeClass("active-a92");
	$(".green").addClass("active-a92");
	$("#tabred1").removeClass("tab-active2");
	$("#tabGreen1").addClass("tab-active2");
}


function changetab3(){
	$(".red").addClass("active-a93");
	$(".green").removeClass("active-a93");
	$("#tabred2").addClass("tab-active2");
	$("#tabGreen2").removeClass("tab-active2");
}
function changetrdTab4(){
	$(".red").removeClass("active-a93");
	$(".green").addClass("active-a93");
	$("#tabred2").removeClass("tab-active2");
	$("#tabGreen2").addClass("tab-active2");
}






$(document).ready(function () {
  $('.tab-a').click(function () {
    $(".tab").removeClass('tab-active9');
    $(".tab[data-id='" + $(this).attr('data-id') + "']").addClass("tab-active9");
    $(".tab-a").removeClass('active-a');
    $(this).parent().find(".tab-a").addClass('active-a');
  });
	
	

	
  $('.tab-btn').click(function () {
    $(".tabs").removeClass('tab-active1');
    $(".tabs[data-id='" + $(this).attr('data-id') + "']").addClass("tab-active1");
    $(".tab-btn").removeClass('active-a1');
    $(this).parent().find(".tab-btn").addClass('active-a1');
  });
  $('.tab-btn2').click(function () {
    $(".tabs2").removeClass('tab-active2');
    $(".tabs2[data-id='" + $(this).attr('data-id') + "']").addClass("tab-active2");
    $(".tab-btn2").removeClass('active-a2');
    $(this).parent().find(".tab-btn2").addClass('active-a2');
  });
  $('ul.market-ul-top li a').click(function () {
    $('ul.market-ul-top li a.active').removeClass('active');
    $(this).addClass('active');
  })
  $('ul.market-ul-top1 li a').click(function () {
    $('ul.market-ul-top1 li a.active').removeClass('active');
    $(this).addClass('active');
  });
  $('a.normal-btn').click(function () {
    $('.Repaying-txt').css('visibility', 'hidden');
    $('.Borrowing-txt').css('visibility', 'hidden');
  });
  $('a.borrow-btn').click(function () {
    $('.Repaying-txt').hide();
    $('.Borrowing-txt').show().css('visibility', 'visible');
  })
  $('a.repay-btn').click(function () {
    $('.Repaying-txt').show().css('visibility', 'visible');
    $('.Borrowing-txt').css('display', 'none');
  })
  $('a.normal-btn1').click(function () {
    $('.Repaying-txt1').css('visibility', 'hidden');
    $('.Borrowing-txt1').css('visibility', 'hidden');
  });
  $('a.borrow-btn1').click(function () {
    $('.Repaying-txt1').hide();
    $('.Borrowing-txt1').show().css('visibility', 'visible');
  })
  $('a.repay-btn1').click(function () {
    $('.Repaying-txt1').show().css('visibility', 'visible');
    $('.Borrowing-txt1').css('display', 'none');
  })
});
$(document).on("click", ".naccs .menu div", function () {
  var numberIndex = $(this).index();
  if (!$(this).is("active")) {
    $(".naccs .menu div").removeClass("active");
    $(".naccs ul li").removeClass("active");
    $(this).addClass("active");
    $(".naccs ul").find("li:eq(" + numberIndex + ")").addClass("active");
    var listItemHeight = $(".naccs ul").find("li:eq(" + numberIndex + ")").innerHeight();
    $(".naccs ul").height(listItemHeight + "px");
  }
});
