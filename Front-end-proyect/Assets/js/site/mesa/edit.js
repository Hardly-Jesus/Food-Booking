const formEditMesa = document.getElementById("form-edit-mesa");
const inputName = document.getElementById("Name");
const inputDescripcion = document.getElementById("descripcion");
const inputCantidadPersona = document.getElementById("cantidadPersona");



const params = new URLSearchParams(window.location.search);
const id = params.get("id");



async function getMesasForUser(){

    try{
        
        const token = localStorage.getItem("token");
        const result = await axios.get(`${config.API_URL}/Api/v1/Mesa/GetById/${id}`,{
            headers: {
               
                Authorization: `Bearer ${token}`
                
            }
        });
          
        llenarFormulario(result.data);
    
    }catch(err){

        console.log("Error al obtener restaurante", err);
    }
}


function llenarFormulario(r) {  
    inputName.value = r.nombre;
    inputDescripcion.value = r.descripcion;
    inputCantidadPersona.value = r.cantidadPersonas;
}



formEditMesa.addEventListener("submit",function(e){
    e.preventDefault();
     editarMesas();
});



async function editarMesas() {

    
    const nombre = inputName.value;
    const descripcion = inputDescripcion.value;
    const cantidadPersonas = inputCantidadPersona.value; 
    const token = localStorage.getItem("token");

    await axios.put(
        `${config.API_URL}/Api/v1/Mesa/Update/${id}`,{
          nombre,
          descripcion,
          cantidadPersonas
        },
        {
            headers: {
                Authorization: `Bearer ${token}`
            }
        }
    );


   return window.location.href = "/Assets/view/Propietario/mesa/Mesas.html";
}



document.addEventListener("DOMContentLoaded", () => {
    getMesasForUser();
});


