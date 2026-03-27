const addMenuForm = document.getElementById("menu-form");
const inputNombre = document.getElementById("nombre");
const inputDescripcion = document.getElementById("descripcion");
const btnSaveMenu = document.getElementById("btn-save-menu");

addMenuForm.addEventListener("submit",function(e){
e.preventDefault();

  addMenu();


});



async function addMenu(){

  try{
      
    const nombre = inputNombre.value;
    const descripcion = inputDescripcion.value;
    const result = await axios.post("https://localhost:7039/Api/v1/Menu/add-menu",{nombre,descripcion});

     if(result.IsCreated){
        btnSaveMenu.setAttribute("disable");
     }
    

     return  window.location.href = "/Assets/view/Propietario/menu/menu.html";

  }catch(err){
    console.log("Ocurrio un error al intentar agregar el menu " + err);
  }
}



