function OnSuccessCadastrarMenu(data) {
    $('#formCadastroMenu').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginCadastrarMenu() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formCadastroMenu").css({ opacity: "0.5" });
}