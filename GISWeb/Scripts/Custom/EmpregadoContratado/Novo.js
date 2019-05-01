AplicaValidacaoCPF();



    $("#txtCPF").keydown(function () {
        try {
            $("#txtCPF").unmask();
        } catch (e) { }

        $("#txtCPF").inputmask("999.999.999-99");

    });

    function OnSuccessCadastrarEmpContratado(data) {
        $('#formCadEmpregadoContratado').removeAttr('style');
        $(".LoadingLayout").hide();
        $('#btnSalvar').show();
        TratarResultadoJSON(data.resultado);
    }

    function OnBeginCadastrarEmpContratado() {
        $(".LoadingLayout").show();
        $('#btnSalvar').hide();
        $("#formCadastroEmpregado").css({ opacity: "0.5" });
    }
