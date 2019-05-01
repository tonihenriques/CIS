AplicaValidacaoCPF();



    $("#txtCPF").keydown(function () {
        try {
            $("#txtCPF").unmask();
        } catch (e) { }

        $("#txtCPF").inputmask("999.999.999-99");

    });

    function OnSuccessCadastrarCat(data) {
        $('#formCadCat').removeAttr('style');
        $(".LoadingLayout").hide();
        $('#btnSalvar').show();
        TratarResultadoJSON(data.resultado);
    }

    function OnBeginCadastrarCat() {
        $(".LoadingLayout").show();
        $('#btnSalvar').hide();
        $("#formCadastroCat").css({ opacity: "0.5" });
    }
