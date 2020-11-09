$(document).ready(function(){
	"use strict";

	/***** Preloader *****/
	$(window).on('load', function() {
		$(".preloader .item-wrapper").delay(500).animate({
	        top: "-100%"
	    },500,"easeInQuart");
	    $(".preloader").delay(500).fadeOut(500);
	});

	/***** Sicky Menu *****/
	$(window).on('scroll',function() {
		var scroll = $(window).scrollTop();
		if (scroll < 200) {
			$(".sticky-menu").removeClass("sticky");
		}else{
			$(".sticky-menu").addClass("sticky");
		}
	});

	/***** Owl Carousel *****/

	// Home 1 Slider
	$(".owl-slider").owlCarousel({
		autoplay:true,
    	autoplayTimeout:7000,
    	animateOut: 'fadeOut',
        animateIn: 'fadeIn',
    	autoplayHoverPause:true,
    	smartSpeed:700,
		loop: true,
		responsiveClass: true,
		items : 1,
		nav : true,
		navText: ['<img src="images/left-arrow.png" alt="" />', '<img src="images/right-arrow.png" alt="" />'],
		margin: 0,
		dots: true
    });

	$(".owl-slider").on('translate.owl.carousel', function() {
		$('.slider-item .img1.effect').removeClass('wow').hide();
        $('.slider-item .img2.effect').removeClass('wow').hide();
        $('.slider-item .slider-box .effect').removeClass('wow').hide();
    });

    $(".owl-slider").on('translated.owl.carousel', function() {
		$('.owl-item.active .slider-item .img1.effect').addClass('animated').show();
        $('.owl-item.active .slider-item .img2.effect').addClass('animated').show();
        $('.owl-item.active .slider-item .slider-box .effect').addClass('animated').show();
    });

	// Home 2 Slider
	$(".slider-wrapper").owlCarousel({
		autoplay:true,
    	autoplayTimeout:7000,
    	animateOut: 'fadeOut',
        animateIn: 'fadeIn',
    	autoplayHoverPause:true,
    	smartSpeed:200,
		loop: true,
		responsiveClass: true,
		items : 1,
		nav : true,
		navText: ['<img src="images/left-arrow.png" alt="" />', '<img src="images/right-arrow.png" alt="" />'],
		margin: 0,
		dots: true
    });

	$(".slider-wrapper").on('translate.owl.carousel', function() {
        $('.slider-item .img1.effect').removeClass('wow').hide();
        $('.slider-item .img2.effect').removeClass('wow').hide();
        $('.slider-item .slider-box .effect').removeClass('wow').hide();
    });

    $(".slider-wrapper").on('translated.owl.carousel', function() {
        $('.owl-item.active .slider-item .img1.effect').addClass('animated').show();
        $('.owl-item.active .slider-item .img2.effect').addClass('animated').show();
        $('.owl-item.active .slider-item .slider-box .effect').addClass('animated').show();
    });

	// Best Deal
	$(".bt-body").owlCarousel({
		autoplay:false,
    	autoplayHoverPause:true,
    	smartSpeed:500,
		loop: true,
		responsiveClass: true,
		items : 1,
		nav : true,
		navText: ['<i class="fa fa-angle-left"></i>', '<i class="fa fa-angle-right"></i>'],
		margin: 0,
		dots: false,
    });

	// Hot Offer
	$(".ht-body").owlCarousel({
		autoplay:false,
    	autoplayHoverPause:true,
    	smartSpeed:500,
		loop: true,
		responsiveClass: true,
		items : 1,
		nav : true,
		navText: ['<i class="fa fa-angle-left"></i>', '<i class="fa fa-angle-right"></i>'],
		margin: 0,
		dots: false,
    });

	// Feature Product Home 1
	$(".tab-slider").owlCarousel({
		autoplay:false,
    	autoplayTimeout:7000,
    	autoplayHoverPause:true,
    	smartSpeed:500,
		loop: true,
		responsiveClass: true,
		items : 3,
		nav : true,
		navText: ['<i class="fa fa-angle-left"></i>', '<i class="fa fa-angle-right"></i>'],
		margin: 20,
		dots: true,
		responsive: {
			0: {
				items: 1
			},
			576: {
				items: 2
			},
			768: {
				items: 2
			},
			992: {
				items: 3
			}
		}
    });

	// New Product Home 1
	$(".new-slider").owlCarousel({
		autoplay:false,
    	autoplayTimeout:7000,
    	autoplayHoverPause:true,
    	smartSpeed:500,
		loop: true,
		responsiveClass: true,
		items : 3,
		nav : true,
		navText: ['<i class="fa fa-angle-left"></i>', '<i class="fa fa-angle-right"></i>'],
		margin: 20,
		dots: true,
		responsive: {
			0: {
				items: 1
			},
			576: {
				items: 2
			},
			768: {
				items: 2
			},
			992: {
				items: 3
			}
		}
    });

	// Top Seller
	$(".slr-slider").owlCarousel({
		autoplay:false,
    	autoplayHoverPause:true,
    	smartSpeed:500,
		loop: true,
		responsiveClass: true,
		items : 3,
		nav : true,
		navText: ['<i class="fa fa-angle-left"></i>', '<i class="fa fa-angle-right"></i>'],
		margin: 20,
		dots: false,
		responsive: {
			0: {
				items: 1
			},
			576: {
				items: 2
			},
			768: {
				items: 2
			},
			992: {
				items: 3
			}
		}
    });

	// Blog Slider
	$(".blog-slider").owlCarousel({
		autoplay:false,
    	autoplayHoverPause:true,
    	smartSpeed:500,
		loop: true,
		responsiveClass: true,
		items : 3,
		nav : true,
		navText: ['<i class="fa fa-angle-left"></i>', '<i class="fa fa-angle-right"></i>'],
		margin: 25,
		dots: false,
		responsive: {
			0: {
				items: 1
			},
			576: {
				items: 2
			},
			768: {
				items: 2
			},
			992: {
				items: 3
			}
		}
    });

	// Testimonial Slider
	$(".test-body").owlCarousel({
		autoplay:true,
    	autoplayHoverPause:true,
    	smartSpeed:500,
		loop: true,
		responsiveClass: true,
		items : 1,
		nav : false,
		margin: 25,
		dots: true,
    });

	//Top Rated Slider
	$(".rt-slider").owlCarousel({
		autoplay:false,
    	autoplayHoverPause:true,
    	smartSpeed:500,
		loop: true,
		responsiveClass: true,
		items : 1,
		nav : true,
		navText: ['<i class="fa fa-angle-left"></i>', '<i class="fa fa-angle-right"></i>'],
		margin: 0,
		dots: false,
    });

	// Brand Slider
	$(".tp-bnd").owlCarousel({
		autoplay:true,
    	autoplayHoverPause:true,
    	smartSpeed:500,
		loop: true,
		responsiveClass: true,
		items : 6,
		nav : false,
		margin: 20,
		dots: false,
		responsive: {
			0: {
				items: 2
			},
			576: {
				items: 3
			},
			768: {
				items: 4
			},
			992: {
				items: 6
			}
		}
    });

	// Best Offer Slider Home 2
	$(".bst-body").owlCarousel({
		autoplay:false,
    	autoplayHoverPause:true,
    	smartSpeed:500,
		loop: true,
		responsiveClass: true,
		items : 1,
		nav : true,
		navText: ['<i class="fa fa-angle-left"></i>', '<i class="fa fa-angle-right"></i>'],
		margin: 0,
		dots: false,
    });

	// Home 2 blog
	$(".fb-slider").owlCarousel({
		autoplay:false,
    	autoplayHoverPause:true,
    	smartSpeed:500,
		loop: true,
		responsiveClass: true,
		items : 3,
		nav : true,
		navText: ['<i class="fa fa-angle-left"></i>', '<i class="fa fa-angle-right"></i>'],
		margin: 25,
		dots: false,
		responsive: {
			0: {
				items: 1
			},
			576: {
				items: 2
			},
			768: {
				items: 3
			},
			992: {
				items: 3
			}
		}
    });

	// Mega Menu Slider
	$(".m-slider").owlCarousel({
		autoplay:true,
    	autoplayHoverPause:true,
    	smartSpeed:500,
		loop: true,
		responsiveClass: true,
		items : 1,
		margin: 0,
		nav : false,
		dots: false
    });

	// Team slider
	$(".team-slider").owlCarousel({
		autoplay:false,
    	autoplayHoverPause:true,
    	smartSpeed:500,
		loop: true,
		responsiveClass: true,
		items : 4,
		nav : true,
		navText: ['<i class="fa fa-angle-left"></i>', '<i class="fa fa-angle-right"></i>'],
		margin: 25,
		dots: false,
		responsive: {
			0: {
				items: 1
			},
			576: {
				items: 2
			},
			768: {
				items: 3
			},
			992: {
				items: 4
			}
		}
    });

	// Similar Item Slider
	$(".sim-slider").owlCarousel({
		autoplay:false,
    	autoplayHoverPause:true,
    	smartSpeed:500,
		loop: true,
		responsiveClass: true,
		items : 4,
		nav : true,
		navText: ['<i class="fa fa-angle-left"></i>', '<i class="fa fa-angle-right"></i>'],
		margin: 25,
		dots: false,
		responsive: {
			0: {
				items: 1
			},
			576: {
				items: 2
			},
			768: {
				items: 3
			},
			992: {
				items: 4
			}
		}
    });

	/***** Shopping Cart *****/
	$('.cart-btn').on('click', function(e){
	    e.preventDefault();
	    $('.cart-overlay').addClass('visible');
	    $('.cart-body').addClass('open');
	});
	$('.close-cart, .cart-overlay').on('click', function(e){
	    e.preventDefault();
	    $('.cart-overlay').removeClass('visible');
	    $('.cart-body').removeClass('open');
	});

	/***** WOW Js *****/
	new WOW().init();

	/***** Tooltip *****/
	$('[data-toggle="tooltip"]').tooltip();

	/***** Price Filter *****/
	$( "#slider-range" ).slider({
		range: true,
		min: 0,
		max: 1000,
		values: [ 200, 800 ],
		slide: function( event, ui ) {
			$( "#amount" ).val( "$" + ui.values[ 0 ] + " - $" + ui.values[ 1 ] );
		}
	});
	$( "#amount" ).val( "$" + $( "#slider-range" ).slider( "values", 0 ) +
	  " - $" + $( "#slider-range" ).slider( "values", 1 ) );

	/***** Quantity Button *****/
	function wcqib_refresh_quantity_increments() {
	  jQuery(".quantity:not(.buttons_added), td.quantity:not(.buttons_added)").each(function(a, b) {
	      var c = jQuery(b);
	      c.addClass("buttons_added"), c.children().first().before('<input type="button" value="-" class="minus" />'), c.children().last().after('<input type="button" value="+" class="plus" />')
	  })
	}
	String.prototype.getDecimals || (String.prototype.getDecimals = function() {
	  var a = this,
	      b = ("" + a).match(/(?:\.(\d+))?(?:[eE]([+-]?\d+))?$/);
	  return b ? Math.max(0, (b[1] ? b[1].length : 0) - (b[2] ? +b[2] : 0)) : 0
	}), jQuery(document).ready(function() {
	  wcqib_refresh_quantity_increments()
	}), jQuery(document).on("updated_wc_div", function() {
	  wcqib_refresh_quantity_increments()
	}), jQuery(document).on("click", ".plus, .minus", function() {
	  var a = jQuery(this).closest(".quantity").find(".qty"),
	      b = parseFloat(a.val()),
	      c = parseFloat(a.attr("max")),
	      d = parseFloat(a.attr("min")),
	      e = a.attr("step");
	  b && "" !== b && "NaN" !== b || (b = 0), "" !== c && "NaN" !== c || (c = ""), "" !== d && "NaN" !== d || (d = 0), "any" !== e && "" !== e && void 0 !== e && "NaN" !== parseFloat(e) || (e = 1), jQuery(this).is(".plus") ? c && b >= c ? a.val(c) : a.val((b + parseFloat(e)).toFixed(e.getDecimals())) : d && b <= d ? a.val(d) : b > 0 && a.val((b - parseFloat(e)).toFixed(e.getDecimals())), a.trigger("change")
	});

	/***** Star Rating *****/
	var $star_rating = $('.star-rating .fa');
	var SetRatingStar = function() {
		return $star_rating.each(function() {
			if (parseInt($star_rating.siblings('input.rating-value').val()) >= parseInt($(this).data('rating'))) {
				return $(this).removeClass('fa-star-o').addClass('fa-star');
			} else {
				return $(this).removeClass('fa-star').addClass('fa-star-o');
			}
		});
	};
	$star_rating.on('click', function() {
		$star_rating.siblings('input.rating-value').val($(this).data('rating'));
		return SetRatingStar();
	});

	/***** Mobile Menu *****/
	jQuery('nav#dropdown').meanmenu({
		meanScreenWidth: "767"
	});

	/***** Back To Top *****/
	$(window).scroll(function(){
		if($(this).scrollTop()>700){
		    $(".back-to-top").fadeIn();
		} else{
		    $(".back-to-top").fadeOut();
		}
	});
	$(".back-to-top").on('click',function(){
	    $("html, body").animate({scrollTop:0}, 700);
	});
});
