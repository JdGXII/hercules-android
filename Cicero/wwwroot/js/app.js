var app = angular.module('ciceroApp', ['ngAnimate']);

app.controller('mainCtrl', function ($scope) {

    var demandante = { nombre: "", apellido: "", dni: "", telefono: "", email: "" }; 
    var demandado = { nombre: "", ruc: "", telefono: "", email: "" };
    var reclamo = { video: "", comentario: "Sin comentario" };
    var solicitud = { seleccion: "", otro: "" };

    $scope.resultado_expediente;
    

   
    function getExpediente() {

        var url = "/Main/getExpediente";
        var exp = $('#numExpediente').val();
        $.get(url, { expediente: exp }, function (data) {
            $("#resultado").val(data);
        });

    }
 

    $scope.demandante = demandante;
    $scope.demandado = demandado;
    $scope.reclamo = reclamo;
    $scope.solicitud = solicitud;


   
});


function previewVideo(input_id, element_id) {

    var preview = document.querySelector(element_id);
    var file = document.querySelector(input_id).files[0];
    var reader = new FileReader();

    reader.onloadend = function () {
        preview.src = reader.result;
    }

    if (file) {
        reader.readAsDataURL(file);
    } else {
        preview.src = "";
    }
}

function previewImage(input_id, display_id) {
    var preview = document.querySelector(display_id);
    var file = document.querySelector(input_id).files[0];
    var reader = new FileReader();

    reader.onloadend = function () {
        preview.src = reader.result;
    }

    if (file) {
        reader.readAsDataURL(file);
    } else {
        preview.src = "";
    }
}

function getExpediente() {

    var url = "/Main/getExpediente";
    var exp = $('#numExpediente').val();
    $.get(url, { expediente: exp }, function (data) {
        
        $("#resultado").html(data);
        $("#resultado").show();
    });

}

function borrarReclamo() {

    $("#resultado").hide();

}

