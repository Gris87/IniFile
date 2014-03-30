if [ -z "$1" ]; then
  echo "Usage: ./commit.sh MESSAGE"  
else
  git add .
  git commit -a -m "$1"
  . push.sh
fi
