$('document').ready(function () {
  LoadTree();
})

function LoadTree() {
  let x = {
    ALedgerNumber: window.localStorage.getItem('current_ledger'),
    AAccountHierarchyCode: 'STANDARD'
  };
  api.post('serverMFinance.asmx/TGLSetupWebConnector_LoadAccountHierarchyHtmlCode', x).then(function (data) {
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

			p = {'ATreeFile': s};
			api.post('serverMFinance.asmx/TGLSetupWebConnector_ImportAccountHierarchy', p)
			.then(function (result) {
				result = result.data;
				if (result != '') {
					if (result.d == true) {
						display_message(i18next.t('AccountTree.uploadsuccess'), "success");
					} else {
						display_message(i18next.t('AccountTree.uploaderror'), "fail");
					}
				}
			})
			.catch(error => {
				display_message(i18next.t('AccountTree.uploaderror'), "fail");
			});
		};
	})(self[0].files[0]);

	// Read in the file as a data URL.
	reader.readAsText(self[0].files[0]);
};

function export_file() {
  let x = {
    ALedgerNumber: window.localStorage.getItem('current_ledger'),
    AAccountHierarchyName: 'STANDARD'
  };
  api.post('serverMFinance.asmx/TGLSetupWebConnector_ExportAccountHierarchyYml', x).then(function (data) {
    var parsed = JSON.parse(data.data.d);
    var _file_ = window.atob(parsed.AHierarchyYml);

    var link = document.createElement("a");
    link.style = "display: none";
    link.href = 'data:text/plain;charset=utf-8,'+_file_;
    link.download = 'AccountTree.csv';
    document.body.appendChild(link);
    link.click();
    link.remove();
  })

}
