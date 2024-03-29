# EasySave

Un logiciel de sauvegarde, conçu pour vous permettre de creer des sauvegardes de vos données en toute simplicité et sécurité.

---
## Instructions d'utilisation :
1. **Lancer l’application.**
    Lorsque vous lancez l'application pour la premiere fois vous serez invité a paramètrer l'application et ainsi choisir la langue, l'emplacement du dossier ou seront efféctués vos sauvegardes en plus de l'emplacement des logs et du fichier d'etat des sauvegardes.
2. **Afficher les sauvegardes :**
   Affiche la liste de vos sauvegardes.
3. **Créer une nouvelle sauvegarde :**
    Vous permet de creer une sauvegarde. L'application propose *deux* types de sauvegardes :
   
    ***Complete :*** Sauvegarde tout les fichiers du dossier source vers le chemin cible. Peu importe ce que le dossier cible contient.
    ***Différentielle:*** Sauvegarde uniquement les fichiers modifiés ou inexistants dans le dossier cible.

5. **Supprimer une sauvegarde :**
    Choisissez simplement l'identifiant de la sauvegarde a supprimer depuis le menu de suppression. Chaque suppression demande une confirmation avant de proceder.

6. **Paramètres :**
   Vous permet de modifier les parametres de l'application, tel que la langue, le dossier cible, et les emplacements des logs.

7. **Quitter :**
   Vous permet de quitter l'application.

---

Sauvegardez vos données avec confiance et simplicité grâce à notre logiciel de sauvegarde efficace. </center>
Merci de faire partie de notre communauté !

---

# EasySave

A backup software designed to allow you to create backups of your data with simplicity and security.

---

## User Guide:
1. **Launch the application.**
    When you launch the application for the first time, you will be prompted to configure the application, choosing the language, the location of the folder where your backups will be made, as well as the location of logs and the backup status file.

2. **Display backups:**
   Displays the list of your backups.

3. **Create a new backup:**
    Allows you to create a backup. The application offers *two* types of backups:
   
    ***Full:*** Backs up all files from the source folder to the target path, regardless of the contents of the target folder.
    ***Differential:*** Backs up only modified or non-existent files in the target folder.

5. **Delete a backup:**
    Simply choose the ID of the backup to delete from the deletion menu. Each deletion requires confirmation before proceeding.

6. **Settings:**
   Allows you to modify application settings, such as language, target folder, and log locations.

7. **Quit:**
   Allows you to exit the application.

---

Back up your data with confidence and simplicity using our efficient backup software.
Thank you for being part of our community!

---

# Release Note - EasySave v1.0 

We're thrilled to announce the successful completion of EasySave v1.0, nearly meeting the comprehensive set of functionalities outlined in the project specifications. This milestone represents a significant achievement in developing a robust backup software adhering to ProSoft's stringent requirements.

## Features Delivered

### Console Application using .Net Core
- Implemented a fully functional console application using .Net Core technology, providing a stable and versatile platform for EasySave.

### Backup Task Management
- Users can create up to 5 distinct backup tasks, defining each task with:
    - Name of the backup
    - Source directory
    - Target directory
    - Type of backup (full, differential)

### Multilingual Support
- Enabled usability for both English and French-speaking users, ensuring accessibility across diverse user bases.

### Source and Target Directory Flexibility
- Implemented compatibility with various directory types including local disks, external disks, and network drives for both source and target directories.

### Comprehensive Backup Inclusions
- Ensured comprehensive backup by including all elements within the source directory, encompassing both files and subdirectories.

### Real-time Log and State Monitoring
- Enabled real-time monitoring and logging of backup activities with detailed information, including:
    - Daily Log File (log.json) capturing action history with timestamps, file addresses, sizes, and transfer times.
    - Real-time State File (state.json) recording progress details of active backup tasks.

### File Formats and Locations
- Adopted JSON format for log, state, and configuration files, facilitating quick readability and ensuring compatibility.
- Designed file locations (log, state) to seamlessly function on client servers, avoiding problematic directory paths.

## What's Next?

We're now focusing on optimizing performance, ensuring code cleanliness, and preparing for potential future developments as per ProSoft's directives.

Stay tuned for future updates as we continue our commitment to delivering efficient, user-friendly, and reliable software solutions.



