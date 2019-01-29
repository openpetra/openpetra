// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
//
// Copyright 2019 by SolidCharity.com
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

var last_opened_entry_data = {};

$(function() {
	var year = (new Date()).getYear() + 1900 - 1;
	$("#DonationStartDate").val(year + "-01-01");
	$("#DonationEndDate").val(year + "-12-31");
});

function calculate_report() {
	let obj = $('#reportfilter');
	// extract information from a jquery object
	let params = extract_data(obj);

	calculate_report_common("forms/Partner/Reports/PartnerReports/AnnualReportWithoutAnnualReceiptRecipients.json", params);
}
