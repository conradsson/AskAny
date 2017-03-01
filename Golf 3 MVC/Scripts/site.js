﻿$(document).ready(function () {
    $(".navbar").hide(0).fadeIn(500)
    $(".body-content").hide(0).delay(450).fadeIn(1000)
    $(".carousel").hide(0).delay(450).fadeIn(1000)
    $(".panel-banstatus").hide();



    //  START FLIKEN-EFFEKTER
    $(".startbtn").click(function () {
        $(".panel-nyheter").show();
        $(".panel-banstatus").hide();
    });
    $(".pill-nyheter").click(function () {
        $(".panel-nyheter").show();
        $(".panel-banstatus").hide();
    });


    $(window).scroll(function () {   // Return-to-top Funktion
        if ($(this).scrollTop() >= 400) {
            $('#return-to-top').fadeIn(200);
        } else {
            $('#return-to-top').fadeOut(200);
        }
    });
    $('#return-to-top').click(function () {  
        $('body,html').animate({
            scrollTop: 0
        }, 300);
    });


    var scrollTo = function (identifier, speed) {  // Scroll-To function
        $('html, body').animate({
            scrollTop: $(identifier).offset().top
        }, speed || 750);
    }
});
