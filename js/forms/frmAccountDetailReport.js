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
    // Sorting
    $("input[name=sorting]").change(function() {
        $('#divsortbycostcentre :input').attr('disabled', $(this).attr('value') != "sortbyreference");
        $('#divsortbyanalysistype :input').attr('disabled', $(this).attr('value') != "sortbyanalysistype");
    });

    $(document).ready(function() {
        $("input[value=sorting]").click();
    });
    // Select Account Codes
    $("input[name=selectaccountcodes]").change(function() {
        $('#divselectrange :input').attr('disabled', $(this).attr('value') != "selectrange");
        $('#divfromlist :input').attr('disabled', $(this).attr('value') != "fromlist");
    });

    $(document).ready(function() {
        $("input[value=selectrange]").click();
    });
    // Select Cost Centre Codes
    $("input[name=selectcostcentrecodes]").change(function() {
        $('#divselectrange-1 :input').attr('disabled', $(this).attr('value') != "selectrange-1");
        $('#divfromlist-1 :input').attr('disabled', $(this).attr('value') != "fromlist-1");
    });

    $("input[value=selectrange-1]").click();
}
