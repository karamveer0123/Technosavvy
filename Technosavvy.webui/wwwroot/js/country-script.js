
$("#cmbIdioma").select2({
        templateResult: function (idioma) {
        var $span = $("<span><img src='https://www.free-country-flags.com/countries/"+idioma.id+"/1/tiny/" + idioma.id + ".png'/> " + idioma.text + "</span>");
        return $span;
        },
        templateSelection: function (idioma) {
        var $span = $("<span><img src='https://www.free-country-flags.com/countries/"+idioma.id+"/1/tiny/" + idioma.id + ".png'/> " + idioma.text + "</span>");
        return $span;
        }
    });
