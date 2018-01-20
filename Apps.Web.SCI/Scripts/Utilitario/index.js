var status = false;

$(document).ready(function () {
    $("#date1").val(obtener_fecha_hoy());
    $("#status").val("");
    $('#load').click(function () { UploadFile(); });
    $('#download').click(function () { EjecuteFile(); });
})

var obtener_fecha_hoy = function() {
    var f = new Date();
    var day = f.getDate();
    var month = f.getMonth() + 1;
    var year = f.getFullYear();
    var fecha = "";

    if(day < 10)
        day = "0" + day;

    if(month < 10)
        month = "0" + month;

    fecha = year + "-" + month + "-" + day;

    return fecha;
}

var validar_fecha = function (fecha) {
    try
    {
        if (validate_fecha_formato_ingles(fecha))
            return true;
        else
            return false;
    }
    catch (err)
    {
        return false;
    }

}

var validar_load_files = function () {
    var texto;
    var value;
    var file;

    $("#status").html("");
    texto = $("#date1").val();
    value = validar_fecha(texto);
    if (value == false)
    {
        texto = "Digite una fecha con el formato 'yyyy-mm-dd'.";
        ver_error(texto)
        $("#date1").focus();
        return false;
    }
    
    if (validate_file("file1") == false)
        return false;

    if (validate_file("file2") == false)
        return false;

    return true;
}

function isValidDate(day, month, year) {
    var dteDate;
    month = month - 1;
    dteDate = new Date(year, month, day);
    return ((day == dteDate.getDate()) && (month == dteDate.getMonth()) && (year == dteDate.getFullYear()));
}

function validate_fecha_formato_ingles(fecha) {
    var patron = new RegExp("^(19|20)+([0-9]{2})([-])([0-9]{1,2})([-])([0-9]{1,2})$");

    if (fecha.search(patron) == 0) {
        var values = fecha.split("-");
        if (isValidDate(values[2], values[1], values[0])) {
            return true;
        }
    }
    return false;
}

function validate_file(file_id) {
    var texto = "";
    var file = $("#" + file_id);
    var date = $("#date1").val();
    if (file.val() == "")
    {
        texto = "Falta seleccionar el " + file_id;
        ver_error(texto)
        $(file).focus();
        return false;
    }
    else {
        texto = file.val();
        if (texto.indexOf(date + ".csv") != -1)
            return true;
        else {
            texto = "Nombre de archivo incorrecto...Porque no termina en '" + date + "'";
            ver_error(texto)
            $(file).focus();
            return false;
        }
    }
}

function UploadFile() {
    $("#rows").html("");
    if ($("#status").html() !== "")
        return;
    var form = $('#form1')[0];
    var dataString = new FormData(form);
    $.ajax({
        url: "/Utilitario/UploadFile",
        type: 'POST',
        success: function (data) {
            status = data.Status;
            if (data.Status)
                ver_confirmacion(data.Message);
            else
                ver_error(data.Message);
            load_grid(data);
        },
        data: dataString,
        cache: false,
        async :false,
        contentType: false,
        processData: false
    });
}

function EjecuteFile() {
    status = false;
    UploadFile();
    if (status === "true" || status === true) {
        DownloadFile();
    }
}

function DownloadFile() {
    $("#rows").html("");
    var form = $('#form1')[0];
    var dataString = new FormData(form);
    $.ajax({
        url: "/Utilitario/DownloadFile",
        type: 'POST',
        success: function (data) {
            if (data.Status){
                ver_confirmacion(data.Message);
                window.location = "/Utilitario/DownloadFile?nameFile=" + "FileResult_" + data.ProcessDate;
            }                         
            else
                ver_error(data.Message);
            load_grid(data);
        },
        data: dataString,
        cache: false,
        async: false,
        contentType: false,
        processData: false
    });
}

function ver_error(message) {
    $("#status").html(message);
    $("#status").css("color", "red");
}

function ver_confirmacion(message) {
    $("#status").html(message);
    $("#status").css("color", "blue");
}

function load_grid(data) {
    $("#rows").html("");
    $("#fields").html("");
    if (data != null)
    {
        var list = [];
        var inyection = "";
        var add = false;
        if (data.ShowList == 1) {
            list = data.ShoppingCartList;
            inyection = "<tr><th>Index</th><th>Transaction_Date</th><th>Payment_Method</th><th>Credit_Card_Number</th><th>Auth_Code</th><th>Transaction_ID</th><th>Error</th></tr>";
        }            
        
        if (data.ShowList == 2) {
            list = data.AuthorizeNetList;
            inyection = "<tr><th>Index</th><th>Transaction_Date</th><th>Payment_Method</th><th>Credit_Card_Number</th><th>Auth_Code</th><th>Transaction_ID</th><th>Error</th><th>Date_Ordered</th><th>Order_Number</th><th>Source_DNIS</th><th>KEYCODE</th><th>BILL_TO_First_Name</th><th>Last_Name</th></tr>";
        }       

        if (list != null && list.length > 0) {
            $.each(list, function (index, value) {
                if (data.Status)
                    add = true;
                else
                    add = !value.Status;

                if (add) {
                    inyection = inyection + "<tr class='odd'>";
                    inyection = inyection + "<td>" + value.Index + "</td>";
                    inyection = inyection + "<td>" + value.Transaction_Date + "</td>";
                    inyection = inyection + "<td>" + value.Payment_Method + "</td>";
                    inyection = inyection + "<td>" + value.Credit_Card_Number + "</td>";
                    inyection = inyection + "<td>" + value.Auth_Code + "</td>";
                    inyection = inyection + "<td>" + value.Transaction_ID + "</td>";

                    if (value.Error != null && value.Error != "") {
                        inyection = inyection + "<td style='color:red'>" + value.Error + "</td>";
                    }
                    else
                        inyection = inyection + "<td></td>";

                    if (data.ShowList == 2) {
                        inyection = inyection + "<td>" + value.Date_Ordered + "</td>";
                        inyection = inyection + "<td>" + value.Order_Number + "</td>";
                        inyection = inyection + "<td>" + value.Source_DNIS + "</td>";
                        inyection = inyection + "<td>" + value.KEYCODE + "</td>";
                        inyection = inyection + "<td>" + value.BILL_TO_First_Name + "</td>";
                        inyection = inyection + "<td>" + value.Last_Name + "</td>";
                    }
                    inyection = inyection + "</tr>"
                }                
            });
        }
       
        if (inyection != "")
            $("#rows").html(inyection);

    }
}