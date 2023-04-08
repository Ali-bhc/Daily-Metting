jQuery(document).ready(function($) {
    var tabwrapWidth= $('.tabs-wrapper').outerWidth();
    var totalWidth=0;
    jQuery("ul li").each(function() { 
      totalWidth += jQuery(this).outerWidth(); 
    });
    if(totalWidth > tabwrapWidth){
      $('.scroller-btn').removeClass('inactive');
    }
    else{
      $('.scroller-btn').addClass('inactive');
    }

    if($("#scroller").scrollLeft() == 0 ){
      $('.scroller-btn.left').addClass('inactive');
    }
    else{
       $('.scroller-btn.left').removeClass('inactive');
    }
		var liWidth= $('#scroller li').outerWidth();
		var liCount= $('#scroller li').length;
		var scrollWidth = liWidth * liCount;

				$('.right').on('click', function(){
          $('.nav-tabs').animate({scrollLeft: '+=200px'}, 300);
          //Console.log($("#scroller").scrollLeft() + " px");
				});
				
				$('.left').on('click', function(){
					$('.nav-tabs').animate({scrollLeft: '-=200px'}, 300);
				});
      scrollerHide()
     
      function scrollerHide(){
        var scrollLeftPrev = 0;
        $('#scroller').scroll(function () {
            var $elem=$('#scroller');
            var newScrollLeft = $elem.scrollLeft(),
                width=$elem.outerWidth(),
                scrollWidth=$elem.get(0).scrollWidth;
            if (scrollWidth-newScrollLeft==width) {
                $('.right.scroller-btn').addClass('inactive');
            }
            else{

                 $('.right.scroller-btn').removeClass('inactive');
            }
            if (newScrollLeft === 0) {
              $('.left.scroller-btn').addClass('inactive');
            }
            else{

                 $('.left.scroller-btn').removeClass('inactive');
            }
            scrollLeftPrev = newScrollLeft;
        });
    }

});

