jQuery(function ($) {

    Chosen();

    $("#UKEmpresa").attr("readonly", "readonly");

    $.validator.unobtrusive.parse(document);

});

function OnSuccessCadastrarDepartamento(data) {
    $(".LoadingLayout").hide();
    $('#blnSalvar').show();

    TratarResultadoJSON(data.resultado);
}

function OnBeginAtualizarDepartamento() {
    $(".LoadingLayout").show();
    $('#blnSalvar').hide();
}