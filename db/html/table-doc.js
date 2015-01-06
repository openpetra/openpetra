// example url string: table-doc.html?table=a_general_ledger_master&module=finance
function loadParameterTable(parameter, anchor) {
    if (parameter.length == 0) {
        // default to these tables for a start
        parameter = "?table=p_partner&group=partner";
    }
    if (parameter.length > 0) {
        var parameters = parameter.substring(1, parameter.length).split("&");
        var param1=parameters[0].split("=");
        var param2=parameters[1].split("=");
        if (parameters[1].length = 0) {
        	parent.frames["table-info"].location.href="tables/"+param1[1]+".html"+anchor;
        }
        else {
        	parent.frames["table-info"].location.href="tables/"+param1[1]+".html"+anchor;
        	parent.tables.location.href="table-doc-tables-"+param2[1]+".html";
        }
        parent.tables.focus();
    }
}

