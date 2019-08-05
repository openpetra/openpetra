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

function failForInternetExplorer()
{
	if (navigator.appName == 'Microsoft Internet Explorer' || navigator.appName == 'Netscape') {
		var userAgent = navigator.userAgent;
		var testForMSIE = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");

		if (testForMSIE.exec(userAgent) != null) {
			return true;
		}
		/*test for Internet Explorer 11*/
		else if (!!userAgent.match(/Trident.*rv\:11\./)) {
			return true;
		}
	}

	return false;
}


$('document').ready(function () {
	if (failForInternetExplorer()) {
		window.location = "/page_for_ie.html";
	}
})

