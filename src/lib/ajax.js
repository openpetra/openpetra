//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Timotheus Pokorra <tp@tbits.net>
//
// Copyright 2017 by TBits.net
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

// set the Content-Type header
// see https://stackoverflow.com/questions/211348/how-to-let-an-asmx-file-output-json
// https://weblogs.asp.net/scottgu/json-hijacking-and-how-asp-net-ajax-1-0-mitigates-these-attacks
// we can only use POST with json and ASP.Net

var api = axios.create({
    baseURL: '/api/',
    responseType: 'json'
});
api.defaults.headers.post['Content-Type'] = 'application/json';
