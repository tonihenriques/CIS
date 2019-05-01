AplicaValidacaoCPF();



    $("#txtCPF").keydown(function () {
        try {
            $("#txtCPF").unmask();
        } catch (e) { }

        $("#txtCPF").inputmask("999.999.999-99");

    });

    function OnSuccessCadastrarAcidenteNBR(data) {
        $('#formCadAcidenteNBR').removeAttr('style');
        $(".LoadingLayout").hide();
        $('#btnSalvar').show();
        TratarResultadoJSON(data.resultado);
    }

    function OnBeginCadastrarAcidenteNBR() {
        $(".LoadingLayout").show();
        $('#btnSalvar').hide();
        $("#formCadastroAcidenteNBR").css({ opacity: "0.5" });
    }
