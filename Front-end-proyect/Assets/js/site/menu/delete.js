const params = new URLSearchParams(window.location.search);
const id = params.get("id");

const btnEliminar = document.getElementById("btnEliminar");



btnEliminar.addEventListener("click", async () => {
    try {

        const token = localStorage.getItem("token");

        await axios.delete(
            `https://localhost:7039/Api/v1/Menu/delete-menu/${id}`,
            {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            }
        );

     
       window.location.href = "/Assets/view/Propietario/menu/menu.html";

    } catch (err)
    {
        console.log("Error al eliminar el menu", err);
    }
});
