AplicaValidacaoCPF();



    $("#txtCPF").keydown(function () {
        try {
            $("#txtCPF").unmask();
        } catch (e) { }

        $("#txtCPF").inputmask("999.999.999-99");

    });

    function OnSuccessCadastrarEstabelecimento(data) {
        $('#formCadEstabelecimento').removeAttr('style');
        $(".LoadingLayout").hide();
        $('#btnSalvar').show();
        TratarResultadoJSON(data.resultado);
    }

    function OnBeginCadastrarEstabelecimento() {
        $(".LoadingLayout").show();
        $('#btnSalvar').hide();
        $("#formCadastroEstabelecimento").css({ opacity: "0.5" });
    }
