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


function changetrdTab11(){
    $("#mSell").addClass("active-a9");
	$("#mBuy").removeClass("active-a9");
	$("#tabred-1").addClass("tab-active9");
	$("#tabGreen-1").removeClass("tab-active9");
}
function changetab11(){
	$("#mSell").removeClass("active-a9");
	$("#mBuy").addClass("active-a9");
	$("#tabred-1").removeClass("tab-active9");
	$("#tabGreen-1").addClass("tab-active9");
}


function changetrdTab12(){

	$(".red").addClass("active-a9");
	$(".green").removeClass("active-a9");
	$("#tabred-2").addClass("tab-active9");
	$("#tabGreen-2").removeClass("tab-active9");
}
function changetab12(){
	$(".red").removeClass("active-a9");
	$(".green").addClass("active-a9");
	$("#tabred-2").removeClass("tab-active9");
	$("#tabGreen-2").addClass("tab-active9");
}

function changetrdTab13(){

	$(".red").addClass("active-a9");
	$(".green").removeClass("active-a9");
	$("#tabred-3").addClass("tab-active9");
	$("#tabGreen-3").removeClass("tab-active9");
}
function changetab13(){
	$(".red").removeClass("active-a9");
	$(".green").addClass("active-a9");
	$("#tabred-3").removeClass("tab-active9");
	$("#tabGreen-3").addClass("tab-active9");
}

function changetrdTab14(){

	$(".red").addClass("active-a9");
	$(".green").removeClass("active-a9");
	$("#tabred-4").addClass("tab-active9");
	$("#tabGreen-4").removeClass("tab-active9");
}
function changetab14(){
	$(".red").removeClass("active-a9");
	$(".green").addClass("active-a9");
	$("#tabred-4").removeClass("tab-active9");
	$("#tabGreen-4").addClass("tab-active9");
}

function changetrdTab15(){

	$(".red").addClass("active-a9");
	$(".green").removeClass("active-a9");
	$("#tabred-5").addClass("tab-active9");
	$("#tabGreen-5").removeClass("tab-active9");
}
function changetab15(){
	$(".red").removeClass("active-a9");
	$(".green").addClass("active-a9");
	$("#tabred-5").removeClass("tab-active9");
	$("#tabGreen-5").addClass("tab-active9");
}

function changetrdTab16(){

	$(".red").addClass("active-a9");
	$(".green").removeClass("active-a9");
	$("#tabred-6").addClass("tab-active9");
	$("#tabGreen-6").removeClass("tab-active9");
}
function changetab16(){
	$(".red").removeClass("active-a9");
	$(".green").addClass("active-a9");
	$("#tabred-6").removeClass("tab-active9");
	$("#tabGreen-6").addClass("tab-active9");
}


function changetrdTab17(){

	$(".red").addClass("active-a9");
	$(".green").removeClass("active-a9");
	$("#tabred-7").addClass("tab-active9");
	$("#tabGreen-7").removeClass("tab-active9");
}
function changetab17(){
	$(".red").removeClass("active-a9");
	$(".green").addClass("active-a9");
	$("#tabred-7").removeClass("tab-active9");
	$("#tabGreen-7").addClass("tab-active9");
}

function changetrdTab18(){

	$(".red").addClass("active-a9");
	$(".green").removeClass("active-a9");
	$("#tabred-7").addClass("tab-active9");
	$("#tabGreen-7").removeClass("tab-active9");
}
function changetab18(){
	$(".red").removeClass("active-a9");
	$(".green").addClass("active-a9");
	$("#tabred-8").removeClass("tab-active9");
	$("#tabGreen-8").addClass("tab-active9");
}

function changetrdTab19(){

	$(".red").addClass("active-a9");
	$(".green").removeClass("active-a9");
	$("#tabred-9").addClass("tab-active9");
	$("#tabGreen-9").removeClass("tab-active9");
}
function changetab19(){
	$(".red").removeClass("active-a9");
	$(".green").addClass("active-a9");
	$("#tabred-9").removeClass("tab-active9");
	$("#tabGreen-9").addClass("tab-active9");
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
