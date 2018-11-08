//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Timotheus Pokorra <tp@tbits.net>
//
// Copyright 2017-2018 by TBits.net
//
// This file is part of OpenPetra.
//
// OpenPetra is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// OpenPetra is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with OpenPetra.  If not, see <http://www.gnu.org/licenses/>.
//

function print_report(UIConnectorUID) {
	let r = {'UIConnectorObjectID': UIConnectorUID, 'AWrapColumn': 'true'};
	api.post('serverMReporting.asmx/TReportGeneratorUIConnector_DownloadText', r).then(function (data) {
		report = data.data.d;
		$('#reporttxt').html("<pre>"+report+"</pre>");
	});
}

function check_for_report(UIConnectorUID) {
	let r = {'UIConnectorObjectID': UIConnectorUID
	};

	api.post('serverMReporting.asmx/TReportGeneratorUIConnector_GetProgress', r).then(function (data) {
		parsed = JSON.parse(data.data.d);
		if (!parsed.JobFinished) {
			// console.log("Report progress: " + parsed.Caption + ' ' + parsed.StatusMessage);
			setTimeout(function() { check_for_report(UIConnectorUID); }, 1000);
		}
		else {
			print_report(UIConnectorUID);
		}
	});
}

function calculate_report_common(params) {
	// now make the parameters into a data table
	param_table = []
	for (var param in params) {
		param_table.push({'name': param, 'value': 'eString:' + params[param], 'column': -1, 'level': -1, 'subreport': -1});
	}

	// send request
	let r = {}
	api.post('serverMReporting.asmx/Create_TReportGeneratorUIConnector', r).then(function (data) {
		let UIConnectorUID = data.data.d;

		let r = {'UIConnectorObjectID': UIConnectorUID,
			'AParameters': JSON.stringify(param_table)
		};

		api.post('serverMReporting.asmx/TReportGeneratorUIConnector_Start', r).then(function (data) {
			setTimeout(function() { check_for_report(UIConnectorUID); }, 1000);
		});
	});
}
