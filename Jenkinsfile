pipeline {
	agent any	  
	  stages {     
	  stage ('Restore packages') {
	    steps {
		    echo 'Restoring packages....'
		    bat 'dotnet restore'
	    }
	  }       
	  stage ('Clean') {
	    steps {
		    echo 'Cleaning the output of the previous build....'
		    bat 'dotnet clean'
	    }
	  }	  	  	   
	  stage ('Build') {
	    steps {
		    echo 'Building the project....'
		    bat 'dotnet build --configuration Release'
	    }
	  }
	  stage ('Test') {
	    steps {
		    echo 'Testing the project....'
		    bat 'dotnet test'
	    }
	  }
	  stage ('Publish framework-dependent') {
	    steps {
		    echo 'Publishing framework-dependent application....'
		    bat 'dotnet publish -f netcoreapp3.1 -c Release --self-contained false'
	    }
	  }	  
	  stage ('Run') {
	    steps {
		    echo 'Running the project....'
		    bat 'dotnet run -p ./DentalApp/DentalApp.csproj &'
	    }
	  }
	}
     }
	  
	    /*
	    //self-contained publishing - czyli publikacja aplikacji samowystarczalnej z całym środowiskiem uruchomieniowym dla win10-x64 // ponad 140 plików ok 90MB
	    stage ('Publish self-contained') {
	      steps {
	        echo "Publishing self-contained application...."
	        bat 'dotnet publish -c Release -r win10-x64'
	    }
	  }
	  */
  

