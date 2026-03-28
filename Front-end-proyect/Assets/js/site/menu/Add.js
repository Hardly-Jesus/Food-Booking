const addMenuForm = document.getElementById("menu-add-form");
const inputNombre = document.getElementById("nombre");
const inputDescripcion = document.getElementById("descripcion");




addMenuForm.addEventListener("submit",function(e){
e.preventDefault();
  addMenu();

});


async function addMenu(){

  try{

    const token = localStorage.getItem("token");
    const nombre = inputNombre.value;
    const descripcion = inputDescripcion.value;
    const result = await axios.post("https://localhost:7039/Api/v1/Menu/add-menu",{nombre,descripcion},
      {
          headers: {
               Authorization: `Bearer ${token}`
          }
      });

   
     return  window.location.href = "/Assets/view/Propietario/menu/menu.html";

  }catch(err){
    console.log("Ocurrio un error al intentar agregar el menu " + err);
  }
}




