AplicaValidacaoCPF();



    $("#txtCPF").keydown(function () {
        try {
            $("#txtCPF").unmask();
        } catch (e) { }

        $("#txtCPF").inputmask("999.999.999-99");

    });

    function OnSuccessCadastrarEmpProprio(data) {
        $('#formCadEmpregadoProprio').removeAttr('style');
        $(".LoadingLayout").hide();
        $('#btnSalvar').show();
        TratarResultadoJSON(data.resultado);
    }

    function OnBeginCadastrarEmpProprio() {
        $(".LoadingLayout").show();
        $('#btnSalvar').hide();
        $("#formCadastroProprio").css({ opacity: "0.5" });
    }
