var app = angular.module('ciceroApp', []);

app.controller('mainCtrl', function ($scope) {

    var demandante = { nombre: "", apellido: "", dni: "", telefono: "", email: "" }; 
    var demandado = { nombre: "", ruc: "", telefono: "", email: "" };
    var reclamo = { video: "", comentario: "Sin comentario" };
    var solicitud = { seleccion: "", otro: "" };


 

    $scope.demandante = demandante;
    $scope.demandado = demandado;
    $scope.reclamo = reclamo;
    $scope.solicitud = solicitud;


   
});


function previewVideo() {

    var preview = document.querySelector('video');
    var file = document.querySelector('#video').files[0];
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

function previewImage() {
    var preview = document.querySelector('#foto-dni');
    var file = document.querySelector('#input-dni').files[0];
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