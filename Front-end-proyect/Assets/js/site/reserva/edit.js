const params = new URLSearchParams(window.location.search);
const id = params.get("id");
const usuarioId = params.get("propietarioId");
const containerInput = document.getElementById("container-input");
const formEdit = document.getElementById("reserva-edit-form");




document.addEventListener("DOMContentLoaded", () => {
      CargarDatos();
    
});



async function CargarDatos(){
     await GetMesas();
     const inputFecha = document.getElementById("fecha");
     const inputHora = document.getElementById("hora");
     const inputMesa = document.getElementById("mesa");

     const data = await GetReserva();

      inputFecha.value = data.fecha.split("T")[0];
      inputHora.value = data.hora.substring(0,5);
      inputMesa.value = data.idMesa;

}


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
                    <div class="mb-3">
                    <label for="fecha" class="form-label">Fecha</label>
                    <input type="date" class="form-control" id="fecha"  name="fecha" required>
                    </div>

                    <div class="mb-3">
                    <label for="hora" class="form-label">Hora</label>
                    <input type="time" class="form-control" id="hora"  name="hora" required>
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




async function GetReserva()
{  
    try{
        
   
        const token = localStorage.getItem("token");
        const resultReserva = await axios.get(`${config.API_URL}/Api/v1/Reserva/get-ById/${id}`,{
            headers: {
                 Authorization: `Bearer ${token}`
            }
        });


        return resultReserva.data;

    }catch(err){

        console.log("Ocurrio un error al obtener el listado de mesas" + err )
    }

}



formEdit.addEventListener("submit",function(e){
   e.preventDefault();

 
        const inputFecha = document.getElementById("fecha");
        const inputHora = document.getElementById("hora");
        const inputMesa = document.getElementById("mesa");

        const fecha = inputFecha.value;
        const hora = inputHora.value;
        const idMesa = inputMesa.value;
     


        EditReserva(fecha,hora,idMesa);

});




async function EditReserva(fecha,hora,idMesa) {

    const token = localStorage.getItem("token");
    await axios.put(
        `${config.API_URL}/Api/v1/Reserva/update-reservas/${id}`,
        {fecha,hora,idMesa},
        {
            headers: {
                Authorization: `Bearer ${token}`
            }
        }
    );


   return window.location.href = "/Assets/view/cliente/reserva/Reserva.html";
}


