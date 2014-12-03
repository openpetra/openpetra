function init()
{
           // Period Range
    $("input[name=periodrange]").change(function() {
        $('#divPeriod :input').attr('disabled', $(this).attr('value') != "period");
        $('#divQuarter :input').attr('disabled', $(this).attr('value') != "quarter");
        $('#divDate :input').attr('disabled', $(this).attr('value') != "date");
    });
    $(document).ready(function() {
        $("input[value=period]").click();
    });


           // Selected Cost Centres
    $("input[name=allradio]").change(function() {
        $('#divSelected_Cost_Centres :input').attr('disabled', $(this).attr('value') != "fromlist");   
    });
    $(document).ready(function() {
        $("input[value=fromlist]").click();
    });


          // Init the radio buttons
    $(document).ready(function() {
        $("input[value=fromlist]").click();
        $('#definecolumn :input').attr('disabled', true);
    });
    $("#idaddbutten").click(function() {
       $('#definecolumn :input').attr('disabled', false);
    });
}
