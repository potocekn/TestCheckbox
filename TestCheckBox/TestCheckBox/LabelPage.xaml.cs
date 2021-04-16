using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppBase
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LabelPage : ContentPage
    {
        public LabelPage()
        {
            InitializeComponent();

            string base64String = Xamarin.Essentials.Preferences.Get("testImage.png", string.Empty);

            string htmlCode = "<!DOCTYPE html>" +
"<html class=\"client-nojs\" lang=\"en\" dir=\"ltr\">" +
"<head>" +
"<meta charset=\"UTF-8\"/>" +
"<title>" + "Baptism - 4training</title>" +

  "</head>" +
  "<body class=\"mediawiki ltr sitedir-ltr mw-hide-empty-elt ns-0 ns-subject page-Baptism rootpage-Baptism skin-fortraining action-view skin-vector-legacy\">" +

              "<h1 id=\"firstHeading\" class=\"firstHeading\" lang=\"en\">" + "Baptism </h1>" +
                                        "<div id=\"bodyContent\" class=\"mw-body-content\">" +
                                        "<div id=\"siteSub\">" + "From 4training</div>" +
                                      "<div id=\"contentSub\">" + "</div>" +
"<img src=\"data:image/png;base64," + base64String + "\" width=\"300\" height=\"200\"></img>" +
"<h2>" + "<span class=\"mw-headline\" id=\"Story\">" + "Story</span>" + "</h2>" +
    "<p>" + "(Matthew 3:11, 13 - 17; 28:18 - 20)" +
"</p>" + "<p>" + "<i>" + "Just before Jesus began to teach and heal people, He went out to the Jordan River to be baptized.A prophet named John was there calling people to turn from their sins because the Savior was coming soon.Jesus was that Savior they had been waiting for!</i>" +
    "</p>" + "<p>" + "<i>" + "Jesus had no sins to repent from, but He wanted to be baptized by John in order to be an example for us to follow and to show that He agreed with John’s message.At first John didn’t want to baptize Jesus and told Him, “I need to be baptized by you!” John knew that Jesus was much greater than him.However, after Jesus told John that it was the right thing to do, John agreed to baptize him.</i>" +
        "</p>" + "<p>" + "<i>" + "John baptized Jesus. So Jesus went down under the water and when He came up out of the water, God’s voice from heaven said, “This is my Son whom I love; with Him I am well pleased.”</i>" +
              "</p>" + "<p>" + "<i>" + "At the end of His ministry on earth, Jesus commanded His followers to go and make disciples of all peoples of the world and to baptize them in the name of the Father, Son, and Holy Spirit.They were also to teach them to obey everything Jesus commanded them. His disciples did as commanded, and everywhere they went, they baptized those who decided to become Jesus’ followers.</i>" +
                   "</p>" + "<p>" + "<b>" + "Practice</b>" + " retelling the story!" +
                          "</p>" +
                          "<h3>" + "<span class=\"mw-headline\" id=\"Questions\">" + "Questions</span>" + "</h3>" +
                              "<ol>" + "<li>" + "What do you learn about baptism from this story ? </li>" +
                                  "<li>" + "What should you obey?</li>" + "</ol>" +
                                    "<h2>" + "<span class=\"mw-headline\" id=\"The_meaning_of_baptism\">" + "The meaning of baptism</span>" + "</h2>" +
                                        "<p>" + "The word “baptism” means “to immerse, submerge” as cleansing or washing. Just as Jesus was baptized, everyone who believes in Him needs to be baptized as well.<br />" +
                                          "Jesus commands His followers at the end of the Gospel of Matthew: <br />" +
"“... baptize them in the name of the Father and of the Son and of the Holy Spirit.” (Matthew 28:19)<br />" +
"The meaning of this verse becomes clear in Acts 2:38(memory verse):" +
"</p>" + "<p>" + "<i>" + "Peter replied, “Each of you must repent of your sins and turn to God, and be baptized in the name of Jesus Christ for the forgiveness of your sins.Then you will receive the gift of the Holy Spirit.”</i>" +
      "</p>" +
      "<p style=\"margin-top:1em;margin-bottom:-1em;text-align:center\">" + "<i>" + "Cleansing in the name of the Father...</i>" + "</p>" +
      "<h3>" + "<span class=\"mw-headline\" id=\"Confessing_sins_and_repentance\">" + "Confessing sins and repentance</span>" + "</h3>" +
      "<p>" + "We confess our sins and turn away from them.We don’t sweep our mistakes under the carpet, but we name and confess them(1 John 1:9).We speak out where we lived against God’s will.We ask God for forgiveness and then stop doing these things.With God’s help we change our thinking and behavior and live according to God’s will." +
    "</p>" +
    "<p style=\"margin-top:1em;margin-bottom:-1em;text-align:center\">" + "<i>" + "Cleansing in the name of the Son...</i>" + "</p>" +
    "<h3>" + "<span class=\"mw-headline\" id=\"Water_baptism_in_the_name_of_Jesus_Christ\">" + "Water baptism in the name of Jesus Christ</span>" + "</h3>" +
    "<p>" + "Water baptism is also called the “washing of rebirth” (Titus 3:5).<br />" +
      "Romans 6:1 - 11 explains this meaning: <br />" +
         "In the same way that Jesus was buried and then rose again to life, we go under water in baptism and come out of the water with a new life.Our old sinful nature dies and we’re no longer “slaves of sin”. That means we no longer have to sin. We’re now a “new creation” (2 Corinthians 5:17). In baptism we bury our old life and our new life begins, a totally new lifestyle guided by the example of Jesus." +
"</p>" +
"<p style=\"margin-top:1em;margin-bottom:-1em;text-align:center\">" + "<i>" + "Cleansing in the name of the Holy Spirit...</i>" + "</p>" +
    "<h3>" + "<span id=\"Receiving_God’s_Spirit\">" + "</span>" + "<span class=\"mw-headline\" id=\"Receiving_God.E2.80.99s_Spirit\">" + "Receiving God’s Spirit</span>" + "</h3>" +
            "<p>" + "God wants to give us His Spirit. The Holy Spirit is like “God’s power” for us: He helps us to do God’s will and to resist the devil.He causes good fruits to grow in us like love, joy, peace and patience(Galatians 5:22).<br />" +
            "When we receive God’s Spirit, something happens in us and this becomes obvious to the outside as well (example: Acts 19:6). We receive supernatural gifts(1 Corinthians 12:1 - 11 and 14:1 - 25). These are a support for us and we use them so that others can experience God’s power as well and we can disciple them." +
               "</p>" +
               "<h2>" + "<span class=\"mw-headline\" id=\"Preparing_for_your_baptism\">" + "Preparing for your baptism</span>" + "</h2>" +
                   "<p>" + "You can celebrate your faith at your baptism!" +
                   "</p>" +
                   "<ul>" + "<li>" + "When should the baptism be ? </li>" +
                   "<li>" + "Whom should we invite ? </li>" +
                   "<li>" + "At your baptism you can prepare your story with God to tell everybody how God rescued and changed you.</li>" + "</ul>" +
                   "<p>" + "Set a time to be baptized as soon as possible.Go through the baptism questions and resolve any questions." +
                   "</p>" +
                   "<h2>" + "<span class=\"mw-headline\" id=\"Baptism_questions\">" + "Baptism questions</span>" + "</h2>" +
                   "<ol>" + "<li>" + "Did you confess your sins to God ? </li>" +
                   "<li>" + "Do you know and believe that God has forgiven all your sins through Jesus’ sacrifice ? </li>" +
                   "<li>" + "Are you ready to bury your old life and start a new life with God ? </li>" +
                   "<li>" + "Are you committed to following Jesus and never turning back ? </li>" +
                   "<li>" + "Will you keep following Jesus even if they mock you, beat you, your family kicks you out, or you’ll have other difficulties?</li>" +
                     "<li>" + "Do you want to receive the Holy Spirit ? </li>" + "</ol>" +
                       "<p>" + "<br />" +
                       "</p>" + "<p>" + "<br />" +
                       "</p>" +
                       "</body>" +
                       "</html>";

            editor.Text = htmlCode;
        }
    }
}