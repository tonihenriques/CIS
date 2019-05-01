AplicaValidacaoCPF();



    $("#txtCPF").keydown(function () {
        try {
            $("#txtCPF").unmask();
        } catch (e) { }

        $("#txtCPF").inputmask("999.999.999-99");

    });

    function OnSuccessCadastrarLesDoen(data) {
        $('#formCadLesDoen').removeAttr('style');
        $(".LoadingLayout").hide();
        $('#btnSalvar').show();
        TratarResultadoJSON(data.resultado);
    }

    function OnBeginCadastrarLesDoen() {
        $(".LoadingLayout").show();
        $('#btnSalvar').hide();
        $("#formCadastroLesDoen").css({ opacity: "0.5" });
    }



    jQuery(function ($) {

        $("#ddlBoletim").change(function () {
            var nome = $("ddlLesao").val;

            if ($(this).val() == 1) {
               
                $("#ddlData").attr("disabled", false);               
               
            }
            else {                
                
                $("#ddlData").attr("disabled", true);
            }

        });

    });



