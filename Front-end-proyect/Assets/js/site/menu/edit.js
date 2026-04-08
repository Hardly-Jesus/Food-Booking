const formEditPlato = document.getElementById("menu-edit-form");
const inputNombre = document.getElementById("nombre");
const inputDescripcion = document.getElementById("descripcion");
 

const params = new URLSearchParams(window.location.search);
const id = params.get("id");


async function getMenuById(){

    try{
        
        const token = localStorage.getItem("token");
        const result = await axios.get(`${config.API_URL}/Api/v1/Menu/get-byId/${id}`,{
            headers: {
               
                Authorization: `Bearer ${token}`
                
            }
        });
       
        
        llenarFormulario(result.data);
    
    }catch(err){

        console.log("Error al obtener el menu", err);
    }
}


function llenarFormulario(r) {
    inputNombre.value = r.nombre;
    inputDescripcion.value = r.descripcion;
  
}



formEditPlato.addEventListener("submit",function(e){
    e.preventDefault();
     editarMenu();


});




async function editarMenu() {

   try{
         
    const nombre = inputNombre.value;
    const descripcion = inputDescripcion.value;
    const token = localStorage.getItem("token");

    await axios.put(
        `${config.API_URL}/Api/v1/Menu/update-menu/${id}`,{nombre,descripcion},
        {
            headers: {
                Authorization: `Bearer ${token}`
            }
        }
    );


      window.location.href = "/Assets/view/Propietario/menu/menu.html";


     }catch(err)   
     {
        console.log("Ocurrio un error al intentar actualizar el menu " + err);

     }
  
}



document.addEventListener("DOMContentLoaded", () => {
   getMenuById();
});



