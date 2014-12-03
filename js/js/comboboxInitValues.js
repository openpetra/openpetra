/// <summary>load values for Combobox via Ajax from the server</summary>
/// <param name="AWebServiceUrl">url of webservice</param>
/// <param name="AWebServiceParameters">array of parameters for webservice</param>
/// <param name="AWithEmptyOption">should there be an empty option in the combobox</param>
/// <param name="AColumnCode">name of the column for the code</param>
/// <param name="AColumnDescription">name of the column for the description</param>
/// <param name="ASelectID">id of the html select element for this combobox</param>
/// <param name="AReturnParameterNumber">the result parameter contains the table at this position</param>
function ComboboxInitValues(AWebServiceUrl, AWebServiceParameters, AWithEmptyOption, AColumnCode, AColumnDescription, ASelectID, AReturnParameterNumber)
{
    AReturnParameterNumber = (typeof AReturnParameterNumber === "undefined") ? 1 : AReturnParameterNumber;
    $.ajax({
      type: "POST",
      url: "../" + AWebServiceUrl,
      data: JSON.stringify(AWebServiceParameters),
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function(data, status, response) {
        result = JSON.parse(response.responseText);
        //console.debug(JSON.stringify(response.responseText));
        console.debug(JSON.stringify(result));
        if (result['d'] != 0)
        {
            //console.debug(JSON.stringify(result['d']));
            //console.debug(JSON.stringify(JSON.parse(result['d'])));
            
            SearchResult = JSON.parse(result['d'])[AReturnParameterNumber];
            
            ParsedSearchResult = new Array();
            
            options = "";
            if (AWithEmptyOption) {
                options = "<option></option>";
            }
            for (index = 0; index < SearchResult.length; ++index) {
                options += "<option value='" + SearchResult[index][AColumnCode] + "'>" + 
                    SearchResult[index][AColumnDescription] + "</option>";
            }
            $(ASelectID).html(options);
        }
        else
        {
            alert("something went wrong");
        }
      },
      error: function(response, status, error) {
        console.debug(error);
        console.debug(JSON.stringify(response.responseJSON));
        alert("Server error, please try again later");
      },
      fail: function(msg) {
        console.debug(msg);
        alert("Server failure, please try again later");
      }
    });      
}
