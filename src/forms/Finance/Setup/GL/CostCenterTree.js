$('document').ready(function () {
  LoadTree();
})

function LoadTree() {
  let x = {
    ALedgerNumber: window.localStorage.getItem('current_ledger')
  };
  api.post('serverMFinance.asmx/TGLSetupWebConnector_LoadCostCentreHierarchyHtmlCode', x).then(function (data) {
    _html_ = data.data.d;
    $('#browse_container').html(_html_);
  })
}

function import_file(file_field) {
  let self = $(file_field);
  var filename = self.val();

	// see http://www.html5rocks.com/en/tutorials/file/dndfiles/
	if (window.File && window.FileReader && window.FileList && window.Blob) {
		//alert("Great success! All the File APIs are supported.");
	} else {
	  alert('The File APIs are not fully supported in this browser.');
	}

	var reader = new FileReader();

	reader.onload = (function(theFile) {
		return function(e) {
			s = e.target.result;

			p = {AYmlHierarchy: s,
				ALedgerNumber: window.localStorage.getItem('current_ledger')};

			api.post('serverMFinance.asmx/TGLSetupWebConnector_ImportCostCentreHierarchy', p)
			.then(function (result) {
				result = result.data;
				if (result != '') {
					var parsed = JSON.parse(result.d);
					if (parsed.result == true) {
						display_message(i18next.t('CostCenterTree.uploadsuccess'), "success");
						LoadTree();
					} else {
						display_error(parsed.AVerificationResult);
					}
				}
			})
			.catch(error => {
				display_message(i18next.t('CostCenterTree.uploaderror'), "fail");
			});
		};
	})(self[0].files[0]);

	// Read in the file as a data URL.
	reader.readAsText(self[0].files[0]);
};

function export_file() {
  let x = {
    ALedgerNumber: window.localStorage.getItem('current_ledger')
  };
  api.post('serverMFinance.asmx/TGLSetupWebConnector_ExportCostCentreHierarchyYml', x).then(function (data) {
	  console.log(data);
    var parsed = JSON.parse(data.data.d);
    console.log(parsed);
    var _file_ = window.atob(parsed.AHierarchyYml);
    var link = document.createElement("a");
    link.style = "display: none";
    link.href = 'data:text/plain;charset=utf-8,'+encodeURIComponent(_file_);
    link.download = i18next.t('CostCenterTree.costcentres_file') + '.csv';
    document.body.appendChild(link);
    link.click();
    link.remove();
  })

}
