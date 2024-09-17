




function activeTabs(id) {
    if (location.search) {
        var dval = $('.' + id).attr('data-parent');
        console.log('dval', dval);

        if (id == dval) {
            nextStep();
            var xx = $('.' + id + ' button');
            xx.addClass('active');
            console.log(xx);                      
        } 
        
       
    }
  
}

function nextStep() {
    var activeStep = $('.active');
    $('.tablinks').removeClass('active');
    $('.accordion-button').removeClass('active active');
    activeStep.next('.tablinks').addClass('active');

    
}

function openCity(evt, cityName) {
    
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
      tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", " ");        
    }
    
    document.getElementById(cityName).style.display = "block";
    activeTabs(cityName);
    evt.currentTarget.className += " active";
    


    
  }

    document.getElementById("defaultOpen").click();
