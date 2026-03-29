const formAddReserva = document.getElementById("reserva-add-form");
const containerInput = document.getElementById("container-input");



const params = new URLSearchParams(window.location.search);
const id = params.get("id");
const idRestaurante = params.get("idRestaurante");





document.addEventListener("DOMContentLoaded", () => {
    GetMesas();
});



async function GetMesas()
{  
    try{
        
        const token = localStorage.getItem("token");
        const result = await axios.get(`https://localhost:7039/Api/v1/Mesa/Get-mesas-byUsuarioId/${id}`,{
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
                    <div class="mb-3">
                    <label for="fecha" class="form-label">Fecha</label>
                    <input type="date" class="form-control" id="fecha"  name="fecha" required>
                    </div>

                    <div class="mb-3">
                    <label for="hora" class="form-label">Hora</label>
                    <input type="time" class="form-control" id="hora" name="hora" required>
                    </div>

                    <div class="mb-3">
                     <label for="mesa" class="form-label">Mesa</label>
                     <select name=idMesa class="form-select" id="mesa">
                      ${data.map(d => 
                           `<option value="${d.id}">${d.nombre} para ${d.cantidadPersonas} personas</option>`
                      ).join('')}                     
                     </select>
                    </div>  
                    `;
    
        containerInput.innerHTML += card;       
}











formAddReserva.addEventListener("submit",function(e){
   e.preventDefault();


         
        const inputFecha = document.getElementById("fecha");
        const inputHora = document.getElementById("hora");
        const inputMesa = document.getElementById("mesa");


        const fecha = inputFecha.value;
        const hora = inputHora.value;
        const idMesa = inputMesa.value;
     


        addReserva(fecha,hora,idMesa);

});




async function addReserva(fecha,hora,idMesa)
{
    try
    {
       
               
       const token = localStorage.getItem("token"); 
       const result = await axios.post("https://localhost:7039/Api/v1/Reserva/add-reservas",{fecha,hora,idMesa,idRestaurante}
        ,{
            headers: {
                Authorization: `Bearer ${token}`
            }
        }
       );

        return window.location.href = "/Assets/view/cliente/reserva/Reserva.html";

    }catch(err){
        
        console.log("Ocurrio un error intentar guardar la reserva " + err);
    }
}



