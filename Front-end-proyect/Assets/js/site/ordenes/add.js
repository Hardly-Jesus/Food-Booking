const formAddOrden = document.getElementById("form-orden-add");
const containerSelectMesas = document.getElementById("mesasContainer");



const params = new URLSearchParams(window.location.search);
const id = params.get("id");
const usuarioId = params.get("usuarioId");





document.addEventListener("DOMContentLoaded", () => {
    GetMesas();
});



async function GetMesas()
{  
    try{
        
        const token = localStorage.getItem("token");
        const result = await axios.get(`${config.API_URL}/Api/v1/Mesa/Get-mesas-byUsuarioId/${usuarioId}`,{
            headers: {
                 Authorization: `Bearer ${token}`
            }
        });


        renderForm(result.data);  

    }catch(err){

        console.log("Ocurrio un error al obtener el listado de mesas" + err )
    }

}










function renderForm(data){
    
        const card = ` 
                     <label for="mesa" class="form-label">Mesa</label>
                     <select name=idMesa class="form-select" id="mesa">
                      ${data.map(d => 
                           `<option value="${d.id}">${d.nombre} para ${d.cantidadPersonas} personas</option>`
                      ).join('')}                     
                     </select>`;
    
        containerSelectMesas.innerHTML += card;       
}











formAddOrden.addEventListener("submit",function(e){
   e.preventDefault();


         
        const inputFecha = document.getElementById("fecha");
        const inputHora = document.getElementById("hora");
        const inputMesa = document.getElementById("mesa");
        const idRestaurante = Number(id);


        const fecha = inputFecha.value;
        const hora = inputHora.value;
        const idMesa = inputMesa.value;
     


        addOrden(fecha,hora,idMesa,idRestaurante);

});




async function addOrden(fecha,hora,idMesa,idRestaurante)
{
    try
    {
       
               
       const token = localStorage.getItem("token"); 
       const result = await axios.post(`${config.API_URL}/Api/v1/Pedido/add-pedido`,{fecha,hora,idMesa,idRestaurante}
        ,{
            headers: {
                Authorization: `Bearer ${token}`
            }
        }
       );

        return window.location.href = "/Assets/view/cliente/ordenes/Ordenes.html";

    }catch(err){
        
        console.log("Ocurrio un error intentar guardar la reserva " + err);
    }
}




