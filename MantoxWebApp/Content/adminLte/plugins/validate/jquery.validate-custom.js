/*! jQuery Validation Plugin - v1.15.0 - 2/24/2016
 * http://jqueryvalidation.org/
 * Copyright (c) 2016 Jörn Zaefferer; Licensed MIT */
 $.validator.addMethod(
    'IPv4',
    function (value) {
    //console.log(value);
    var x = "(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3}";
    //console.log(value.match(x));
    console.log(value.length);
    if(value.length != 15){return false}

    return value.match(x);
    },
	'Dirección IPv4 no v&aacute;lida'
);

 $.validator.addMethod(
    'host',
    function (value) {
    console.log(value);
    var x = "^((-[a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\-]*[A-Za-z0-9])$";
    console.log(value.match(x));
    return value.match(x);
    },
	'Nombre de host no v&aacute;lido'
);

$(document).ready(function(){
    // initialize tooltipster on form ids starting with formulario_crear_
    $('form[id^=formulario_crear_] *').tooltipster({
        trigger: 'custom',
        onlyOne: true,
        position: 'bottom'
    });
});

 
var ValidatorErrorPlacement = function (error, element) {
    $(element).tooltipster('update', $(error).text());
    $(element).tooltipster('show');
};

var ValidatorSuccess = function (label, element) {
    $(element).tooltipster('hide');
};
