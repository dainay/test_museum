# Musée Virtuel - APF France Handicap
https://github.com/dainay/test_museum/

## Contexte du Projet
Ce projet est une collaboration avec APF France Handicap visant à créer un musée virtuel immersif et accessible. Le but est de mettre en lumière les œuvres d'artistes en situation de handicap. Le musée propose deux versions : une complète avec mini-jeux et interactions riches, et une version classique pour la consultation directe des œuvres.

## Équipe et Organisation
L'équipe de développement est composée de six membres : Caroline, Daria, Arthur, Néroli, Waldi, et Manu. Nous utilisons Git pour la gestion de version avec une branche principale unique. Les outils principaux sont Unity (C#) pour le développement et Blender pour la modélisation 3D.

L'équipe créative est composée de Solènne, Andréa, Hugo, et Pauline. Ils s'occupent de la communication, de la prise d'interviews des artistes et des créations graphiques.

## Objectifs Stratégiques et Public Cible
Les objectifs principaux sont :
- **Accessibilité** : Permettre à tous, y compris les personnes handicapées et isolées, de visiter le musée.
- **Sensibilisation** : Valoriser les artistes souvent invisibilisés.
- **Diffusion** : Utiliser le musée comme outil pédagogique pour l'APF et ses partenaires culturels.

Public cible :
- Amateurs d'art et curieux.
- Jeunes (12-40 ans).

## Fonctionnalités Clés
- **Navigation et Interactions** :
  - Déplacement libre dans le musée avec clavier/souris.
  - Interactions avec les œuvres (affichage d'informations, vidéos avec sous-titres).
- **Accessibilité** :
  - Vidéos systématiquement sous-titrées.
- **Gestion des Salles** :
  - Salle Principale (Hub) reliant 6 salles thématiques :
    - Salle Blanche : Collecte de sphères formant une sphère lumineuse.
    - Salle Rose : Déverrouillage d'un coffre par code caché.
    - Salle Noire : Association de sphères texturées à des œuvres.
    - Salle Verte : Récupération d'objet avec changement de couleur.
    - Salle Bleue : Recherche des mots-clés liés aux peintures.
    - Salle Jaune : Associer les descriptions à chaque peinture.
  - Salle secrète contenant une vidéo de remerciement lorsque le joueur a obtenu toutes les sphères finales.
  - Salle d'introduction qui lance une vidéo explicative en mode didacticiel. Le joueur a le choix entre faire l'expérience classique ou l'expérience immersive en prenant une des deux portes correspondantes.
- **Menu Principal** : Ajout d'un menu principal pour naviguer dans le musée et cahnger à tous moment de type d'expérience.

## Structure du Projet
Le projet est organisé autour des salles thématiques et de la salle principale. Chaque salle a ses propres scripts pour gérer les interactions et les mini-jeux. Le code est structuré de manière à faciliter l'ajout de nouvelles salles ou fonctionnalités.

## Technologies Utilisées
- **Unity** (C#) pour le développement du jeu.
- **Blender** pour la modélisation 3D.

## Installation et Configuration
1. Cloner le dépôt Git.
2. Importer le projet dans Unity.
3. Configurer les assets et les scènes dans Unity.
4. Exécuter le projet en mode éditeur pour tester les différentes salles.
5. *Faire le build du projet pour obtenir un fichier .exe et tester le projet en mode plein écran.

## Contribution et Maintenance
- Contribuer via la branche principale unique sur Git.
- Respecter les conventions de codage et les meilleures pratiques définies par l'équipe.
- Effectuer des commits fréquents pour éviter les conflits.

## Critères de Réussite et Risques
- **Critères de Réussite** :
  - Accessibilité (sous-titres, audio).
  - Expérience utilisateur de qualité (navigation fluide, ambiance cohérente).
  - Stabilité et performance (temps de chargement, framerate).
  - Satisfaction des parties prenantes (APF, partenaires).
- **Principaux Risques et Mesures** :
  - Communication : Réunions hebdomadaires, mises à jour régulières sur Notion/Trello.
  - Conflits Git : Commits fréquents.
  - Performances des assets : Optimisation (compression, gestion raisonnée des ressources).
