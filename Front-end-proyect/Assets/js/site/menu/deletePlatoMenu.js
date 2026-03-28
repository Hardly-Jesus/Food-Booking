
const params = new URLSearchParams(window.location.search);
const idPlato = params.get("idPlato");
const idMenu = params.get("idMenu");

const btnEliminar = document.getElementById("btnEliminar");



btnEliminar.addEventListener("click", async () => {
    try {

        const token = localStorage.getItem("token");

        await axios.delete(
            `https://localhost:7039/Api/v1/PlatoMenu/delete-plato-del-menu/${idPlato}/${idMenu}`,
            {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            }
        );

     
       window.location.href = "/Assets/view/Propietario/menu/menu.html";

    } catch (err)
    {
        console.log("Error al eliminar plato del menu", err);
    }
});

