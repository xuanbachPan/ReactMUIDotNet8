import React, { Component, ReactNode, useEffect } from 'react'
import ReactDOM from 'react-dom/client'
import { Button, Stack, TextField } from '@mui/material'
import './InputForm.css';
import 'bootstrap/dist/css/bootstrap.css';
import {Config} from './Config'


let m : number, n : number, p : number;
let htmlTbl : string;
let temp : number;
let treasureFlg : number;
let treasureId : string ;
const root = ReactDOM.createRoot(document.getElementById('root') as HTMLElement);
let MapTblRoot: ReactDOM.Root | null = null;
let ResultMapTblRoot: ReactDOM.Root | null = null;
let API_URL : string;

const MInputHandler = (event : any) => {
    temp =  event.target.value;
    treasureFlg = 0;
    if(temp >= 1 && temp <= 500)
    {
        m = temp;
    }
    else if(temp < 1)
    {
        m=1;
        event.target.value = 1;
    }
    else if(temp > 500)
    {
        m=500;
        event.target.value = 500;
    }
    if(n == undefined) n = 1;
    if(p == undefined) p = 1;
    const element = genInputTable(n, m, p);
    if(MapTblRoot == null)
         MapTblRoot = ReactDOM.createRoot(document.getElementById("OuterMapTable") as HTMLLIElement);       
    MapTblRoot.render(element);
};
const NInputHandler = (event : any) => {
    temp =  event.target.value;
    treasureFlg = 0;
    if(temp >= 1 && temp <= 500){
        n = temp;
    }
    else if(temp < 1)
    {
        n=1;
        event.target.value = 1;
    }
    else if(temp > 500)
    {
        n=500;
        event.target.value = 500;
    }
    if(m == undefined) m = 1;
    if(p == undefined) p = 1;
    const element = genInputTable(n, m, p);
    if(MapTblRoot == null)
         MapTblRoot = ReactDOM.createRoot(document.getElementById("OuterMapTable") as HTMLLIElement);
        
    MapTblRoot.render(element);
};
const PInputHandler = (event : any) => {
    temp =  event.target.value;
    treasureFlg = 0;
    if(n == undefined) n = 1;
    if(m == undefined) m = 1;
    if(temp >= 1 && temp <= m*n){
        p = temp;
    }
    else if(temp < 1)
    {
        p=1;
        event.target.value = 1;
    }
    else if(temp > m*n)
    {
        p=n*m;
        event.target.value = n*m;
    }

    const element = genInputTable(n, m, p);
    if(MapTblRoot == null)
         MapTblRoot = ReactDOM.createRoot(document.getElementById("OuterMapTable") as HTMLLIElement);
        
    MapTblRoot.render(element);

};

document.addEventListener('click', function(event : any){
    for(var i = 1; i <= n; i++){
        for(var j = 1; j <=m; j++){
            const tblCell = document.getElementById('tblCell_' + i + '_' + j);
            if(tblCell){
                tblCell.addEventListener('change',TblCellHandler);
            }
        }
    }   
});

    const TblCellHandler = (event: any) => {
        if(p == undefined) p = 1;
        if(treasureFlg == undefined) treasureFlg = 0;
        if(treasureId == undefined) treasureId = '';
        temp =  event.target.value.replace(/^0+/, '');
        temp = Math.floor(temp);
        if(temp >= p && treasureFlg == 0){
            event.target.value = p;
            treasureFlg = 1;
            treasureId = event.target.id;
        }
        else if(temp >= p && treasureFlg == 1 && event.target.id == treasureId){
            event.target.value = p;
        }
        else if(temp >= p && treasureFlg == 1 && event.target.id != treasureId){
            event.target.value = p - 1;
        }
        else if(temp < p && event.target.id == treasureId){
            treasureFlg = 0;
        }
        else if(temp <= 0){
            event.target.value = 0;
        }
        else{
            event.target.value = temp;
        }
    }   

const genInputTable = (n : number, m : number, p: number) =>{
    let htmlTable : string = "";
    for (var i = 0; i < n; i++){
        htmlTable += "<tr class='tbl-row'>";
        for(var j = 0; j < m; j++){
            htmlTable += "<td ><input class='tbl-cell' id='tblCell_" + (i+1)+ "_" + (j+1) + "'type='number' min='0' max='" + (p) + "'/></td>";
        }
        htmlTable += "</tr>";
    }
    return (
        <table className="inner-map-table" id="InnerMapTable" dangerouslySetInnerHTML={{__html: htmlTable}}></table>
    )
};

const GenResultTable = (n: number, m: number, p: number, data : string) => {
    let htmlTable : string = "";
    let Obj = JSON.parse(data);
    let tblCell : any;
    let minValueFuel = Obj.FuelVal;
    let routeMinValueFuel : string = "";

    Object.keys(Obj.IslandsList).forEach(function(key) {
        if(key == '0'){
            routeMinValueFuel += 'M[' + Obj.IslandsList[key].X + '-' + Obj.IslandsList[key].Y + '](Key: ' + Obj.IslandsList[key].KeyNumber + ')';
        }
        else{
            routeMinValueFuel += ' ==> M[' + Obj.IslandsList[key].X + '-' + Obj.IslandsList[key].Y + '](Key: ' + Obj.IslandsList[key].KeyNumber + ')';
        }       
    });
        
    let redBorder : number = 0;
    for(var i = 0; i < n; i++)
    {
        htmlTable += "<tr class='tbl-row'>";
        for(var j = 0; j < m; j++){
            tblCell = document.getElementById('tblCell_' + (i+1) + '_' + (j+1));
            redBorder = 0;
            Object.keys(Obj.IslandsList).forEach(function(key) {
                if(Obj.IslandsList[key].X == i+1 && Obj.IslandsList[key].Y == j+1){
                    redBorder = 1;
                }
            });    
            if(redBorder == 1 && (tblCell.value == undefined || tblCell.value == null || tblCell.value == '')){
                htmlTable += "<td><input disabled class='result-tbl-cell border-red' type='number' id='resultTblCell_" + (i+1) + "_" + (j+1) + "'/></td>";
            }
            else if(tblCell.value == undefined || tblCell.value == null || tblCell.value == '')
            {
                htmlTable += "<td><input disabled class='result-tbl-cell' type='number' id='resultTblCell_" + (i+1) + "_" + (j+1) + "'/></td>";
            }
            else if(redBorder == 1){
                htmlTable += "<td><input disabled class='result-tbl-cell border-red' type='number' id='resultTblCell_" + (i+1) + "_" + (j+1) + "' value='" + tblCell.value + "'/></td>";
            }
            else{
                htmlTable += "<td><input disabled class='result-tbl-cell' type='number' id='resultTblCell_" + (i+1) + "_" + (j+1) + "' value='" + tblCell.value + "'/></td>";
            }     
        }
        htmlTable += "</tr>";
    }

    return (
        <div>
            <div className="min-value-fuel"><p>Min Value of Used Fuel: {minValueFuel}</p></div>
            <div className="route-min-value-fuel">The Route: {routeMinValueFuel}</div>
            <table className="inner-result-map-table" id="InnerResultMapTable" dangerouslySetInnerHTML={{__html: htmlTable}}></table>
        </div>   
    )

}

const coloringRoute = (data : string) => {

    let Obj = JSON.parse(data);
    let cellId : string = "";
    let resulTblCell : any;
    
        Object.keys(Obj.IslandsList).forEach(function(key) {
            cellId = "resultTblCell_" + Obj.IslandsList[key].X + "_" +  Obj.IslandsList[key].Y + "";
            resulTblCell = document.getElementById(cellId);
            if(resulTblCell != null && resulTblCell != undefined){
                resulTblCell.style.borderColor = "red";
            }
        });
    
}

let outPutJson : any = {};
const submitHandler = () =>{
    let inputTblCellsList : string = '';
    let tblCell : any;
    if(m == undefined) m = 1;
    if(n == undefined) n = 1;
    if(p == undefined) p = 1;
    let q : number = p - 1; 
 
    for (var i = 1; i <= n; i++){
        for (var j = 1; j <= m; j++){
            tblCell =  document.getElementById('tblCell_' + i + '_' + j);
            if(tblCell.value == undefined || tblCell.value == null || tblCell.value == ''){
                if(i == n && j == m)
                    inputTblCellsList += '0';
                else
                    inputTblCellsList += '0,';
            }
            else{
                if(i == n && j == m)
                    inputTblCellsList += tblCell.value;
                else
                    inputTblCellsList += tblCell.value + ',';
            }
        }
    }
    
    if(p > m*n){
        alert('p > m*n. Please set inputs m, n, p again.');
        return;
    }
    else if(treasureFlg == 0 || treasureFlg == undefined){
        alert('You don\'t set any table cell input has value = p(' + p + ').');
        return;
    }

    let checkInputTblCellFlg : number = 0;
    while(q > 0){
        checkInputTblCellFlg = 0;
        for (var i = 1; i <= n; i++){
            for (var j = 1; j <= m; j++){
                tblCell =  document.getElementById('tblCell_' + i + '_' + j);
                if(tblCell.value == q){
                    checkInputTblCellFlg++;
                    break;
                }
            }
            if(checkInputTblCellFlg > 0){
                break;
            }              
        }
        if(checkInputTblCellFlg == 0){
            alert('You don\'t set any table cell input has value = ' + q + '.');
            return;
        }
        q--;
    }

    //console.log('Config.API_URL/ReactMuiAPI/FindTheMinFuel: ' + API_URL + 'ReactMuiAPI/FindTheMinFuel');

    fetch
    (API_URL + 'ReactMuiAPI/FindTheMinFuel?rows=' + n + '&cols=' + m + '&pVal=' + p + '&data=' + inputTblCellsList ,
        {
            method: 'POST',
            headers: {
                'Accept':'application/json',
                'Content-Type':'application/json',
                'Access-Control-Allow-Origin':'*'
            },
            body: JSON.stringify({})
        }
    )
    .then(res=>res.json())
    .then((result)=>{
        if(result.stsCode == 0){
            //alert('Find the min fuel successful.');
            const element = GenResultTable(n,m,p ,result.data);
            if(ResultMapTblRoot == null)
                ResultMapTblRoot = ReactDOM.createRoot(document.getElementById("OuterResultMapTable") as HTMLLIElement);
            ResultMapTblRoot.render(element);
            //coloringRoute(result.data);
            let notice_div : any = document.getElementById('notice_div');
            notice_div.innerHTML = "";

            if(result.stsMessage == 'Duplicate'){
                const pElement = document.createElement('p');
                const content = document.createTextNode("Input and Output data exists in DB. Not save duplicate data.");
                pElement.appendChild(content);
                notice_div.appendChild(pElement);
            }else{
                const pElement = document.createElement('p');
                const content = document.createTextNode("Save data successful.");
                pElement.appendChild(content);
                notice_div.appendChild(pElement);
            }
        }
        else{
            alert('There\'s a problem when find the min fuel. Please contact with the admin.');
        }
       },
       (error)=>{
           if(error)
               alert('Failed! Error: ' + error);
       }
       )   
    }

    export default class InputForm extends Component {

    constructor(props : any){
        super(props);
        API_URL = Config.API_URL;
    }

    render(){
        return (
            
            <div> 
                <div className='submitForm'>
                    <h3 className='title'>Input Field</h3>
                    <Stack direction="row" spacing={2} className='outer-stack'>
                        <Stack direction="row" spacing={2} className='left-stack'>
                            <div className='textfield'>
                                <TextField 
                                    label="M Input (Columns)"
                                    required
                                    type = "number"
                                    id="inputM" 
                                    value={m}
                                    defaultValue={1}
                                    variant='outlined' 
                                    helperText="1 <= M <= 500"
                                    inputProps={{
                                        min: 1, max: 500, style:{textAlign: 'right'}
                                    }} 
                                    onChange={MInputHandler}
                                />
                            </div>
                            <div className='textfield'>
                                <TextField 
                                    label="N Input (Rows)" 
                                    required
                                    type = "number"
                                    id="inputN" 
                                    value={n}
                                    defaultValue={1}
                                    variant='outlined' 
                                    helperText="1 <= N <= 500"
                                    inputProps={{
                                        min: 1, max: 500, style:{textAlign: 'right'}
                                    }}
                                    onChange={NInputHandler}
                                />
                            </div>
                            <div className='textfield'>
                                <TextField 
                                    label="P Input (M*N)"
                                    required
                                    type = "number"
                                    id="inputP" 
                                    value={p}
                                    defaultValue={1}
                                    variant='outlined' 
                                    helperText="1 <= P <= M*N"
                                    inputProps={{
                                        min: 1, style:{textAlign: 'right'}
                                    }}
                                    onChange={PInputHandler}
                                />
                            </div>
                        </Stack>
     
                    </Stack>
                    
    
                    <div className="outer-map-table" id="OuterMapTable">
                        <table>
                            <tr><td><input className='tbl-cell' id='tblCell_1_1' type='number' min='0' max='1' onChange={TblCellHandler}/></td></tr>
                        </table>
                    </div>
                    <button className="Button btn btn-primary" onClick={submitHandler}>Find the min fuel</button>
                    <div id="notice_div"></div>
                    <div className="result-title"><h4>Result</h4></div>
                    <div className ="outer-result-map-table" id="OuterResultMapTable" >                
                    </div>
                </div>
            </div>
        );

    }
    
}