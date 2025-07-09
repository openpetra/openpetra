//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
//
// Copyright 2017-2018 by TBits.net
// Copyright 2019-2025 by SolidCharity.com
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

import i18next from 'i18next'
import api from './ajax.js'
import axios from 'axios';

class Reports {

	constructor() {
		let self = this;
		this.myPleaseWaitDiv = self.initPleaseWaitDiv();

		// for the report parameters json file
		self.src = axios.create({
			baseURL: '/src/',
			responseType: 'json'
		});
		self.src.defaults.headers.get['Content-Type'] = 'application/json';
	}

	initPleaseWaitDiv() {
		var pleaseWaitDiv = $('<div class="modal fade" tabindex="-1" role="dialog" id="spinnerModal"><div class="modal-dialog modal-dialog-centered text-center" role="document"><span class="fas fa-spinner fa-spin fa-3x w-100"></span></div></div>');
		return {
			showPleaseWait: function() {
				pleaseWaitDiv.modal('show');
			},
			hidePleaseWait: function () {
				pleaseWaitDiv.modal('hide');
			},
		};
	}

	modal_spinner(){
		$('.modal').modal('show');
	}

	download_pdf() {
		let ReportID = window.localStorage.getItem('current_report_ReportID');
		let r = {'AReportID': ReportID, 'AWrapColumn': 'true'};
		api.post('serverMReporting.asmx/TReportGeneratorWebConnector_DownloadPDF', r).then(function (data) {
			let report = data.data.d;
			var link = document.createElement("a");
			link.style = "display: none";
			link.href = 'data:application/pdf;base64,'+report;
			link.download = i18next.t('Report') + '.pdf';
			document.body.appendChild(link);
			link.click();
			link.remove();
		});
	}

	download_excel() {
		let ReportID = window.localStorage.getItem('current_report_ReportID');
		let r = {'AReportID': ReportID};
		api.post('serverMReporting.asmx/TReportGeneratorWebConnector_DownloadExcel', r).then(function (data) {
			let report = data.data.d;
			var link = document.createElement("a");
			link.style = "display: none";
			link.href = 'data:application/excel;base64,'+report;
			link.download = i18next.t('Report') + '.xlsx';
			document.body.appendChild(link);
			link.click();
			link.remove();
		});
	}

	print_report(AReportID) {
		self = this;
		let r = {'AReportID': AReportID, 'AWrapColumn': 'true'};
		api.post('serverMReporting.asmx/TReportGeneratorWebConnector_DownloadHTML', r).then(function (data) {
			let report = data.data.d;
			$('#reporttxt').html(report);
		});
		window.localStorage.setItem('current_report_ReportID', AReportID);
		$('#btnDownloadExcel').show();
		$('#btnDownloadPDF').show();
		self.myPleaseWaitDiv.hidePleaseWait();
	}

	check_for_report(AReportID) {
		let self = this;
		let r = {'AReportID': AReportID
		};

		api.post('serverMReporting.asmx/TReportGeneratorWebConnector_GetProgress', r).then(function (data) {
			let parsed = JSON.parse(data.data.d);
			if (!parsed.result.JobFinished) {
				// console.log("Report progress: " + parsed.Caption + ' ' + parsed.StatusMessage);
				setTimeout(function() { self.check_for_report(AReportID); }, 1000);
			} else {
				api.post('serverMReporting.asmx/TReportGeneratorWebConnector_GetSuccess', r).then(function (data) {
					let parsed = JSON.parse(data.data.d);
					if (parsed == true) {
						self.print_report(AReportID);
					} else {
						api.post('serverMReporting.asmx/TReportGeneratorWebConnector_GetErrorMessage', r).then(function (data) {
							utils.display_message(i18next.t(data.data.d), "fail");
							self.myPleaseWaitDiv.hidePleaseWait();
						});
					}
				});
			}
		});
	}

	calculate_report_common(report_common_params_file, specific_params) {
		let self = this;
		// now make the parameters into a data table
		let param_table = []
		for (var param in specific_params) {
			if (typeof specific_params[param] === "boolean") {
				specific_params[param] = "eBoolean:" + specific_params[param];
			}
			else if (specific_params[param] instanceof Date) {
				let strDate = specific_params[param].toISOString();
				// transform from 2018-11-24T21:10:23.922Z to 2018-11-24T21:10:23
				strDate = strDate.substr(0,"2018-11-24T21:10:23".length);
				specific_params[param] = 'eDateTime:"' + strDate + '"';
			}
			else
			{
				specific_params[param] = "eString:" + specific_params[param];
			}
			param_table.push({'name': param, 'value': specific_params[param], 'column': -1, 'level': -1});

		}

		if (report_common_params_file != '') {
			self.src.get(report_common_params_file, {}).then(function (data) {
				for (var param in data.data) {
					// only if parameter does not exist yet
					let exists = false;
					if (data.data[param].column === undefined) { data.data[param].column = -1; }
					for (var existing in param_table) {
						if ((param_table[existing].name == data.data[param].name) && (param_table[existing].column == data.data[param].column)) {
							exists = true;
						}
					}
					if (!exists) {
						param_table.push({'name': data.data[param].name, 'value': data.data[param].value, 'column': data.data[param].column, 'level': -1, 'subreport': -1});
					}
				}

				self.start_calculate_report(param_table);
			});
		} else {
			self.start_calculate_report(param_table);
		}
	}

	start_calculate_report(param_table) {
		let self = this;
		// send request
		let r = {}
		api.post('serverMReporting.asmx/TReportGeneratorWebConnector_Create', r).then(function (data) {
			let ReportID = data.data.d;

			let r = {'AReportID': ReportID,
				'AParameters': JSON.stringify(param_table)
			};

			api.post('serverMReporting.asmx/TReportGeneratorWebConnector_Start', r).then(function (data) {
				self.myPleaseWaitDiv.showPleaseWait();
				setTimeout(function() { self.check_for_report(ReportID); }, 1000);
			});
		});
	}

}

export default new Reports();